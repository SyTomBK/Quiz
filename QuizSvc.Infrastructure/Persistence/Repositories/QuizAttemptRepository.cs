using QuizSvc.Application.Commands.QuizAttempts;
using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.QuizAttempts;

namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class QuizAttemptRepository : IQuizAttemptRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public QuizAttemptRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<QuizAttemptMiniResponseDto> CreateQuizAttempt(CreateQuizAttemptCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        var setting = await _context.CampaignQuizSettings.FindAsync([request.CampaignQuizSettingId], cancellationToken);

        if (setting == null)
            throw GrpcExceptions.NotFound("CampaignQuizSetting", request.CampaignQuizSettingId);

        var maxAttempts = setting.MaxAttempts;

        var completedCount = await _context.QuizAttempts
            .Where(x => x.CampaignQuizSettingId == request.CampaignQuizSettingId && x.AttemptStatus == AttemptStatus.Completed)
            .Where(x => request.UserId != null ? x.UserId == request.UserId : x.LeadId == request.LeadId)
            .CountAsync(cancellationToken);

        if(maxAttempts > 0 && completedCount >= maxAttempts)
            throw GrpcExceptions.InvalidArgument($"Maximum attempts of {maxAttempts} has been reached.");
        
        User? user = null;
        Lead? lead = null;
        
        // login, người dùng đã đăng nhập SSO và lấy userId truyền xuống BE
        if(request.UserId.HasValue)
        {
            user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == request.UserId.Value 
                    && x.Tenant == request.Tenant, cancellationToken);

            if(user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId.Value,
                    Username = request.Username ?? "",
                    Tenant = request.Tenant ?? "",
                    IsActive = true
                };
                await  _context.Users.AddAsync(user, cancellationToken);
            }

            // Nếu login mà có LeadId -> load lead để merge
            if(request.LeadId.HasValue)
            {
                lead = await _context.Leads
                    .FirstOrDefaultAsync(x => x.LeadId == request.LeadId.Value, cancellationToken);

                if(lead != null)
                {
                   await MergeLeadToUserAsync(user, lead, cancellationToken);
                }
                else
                {
                    lead = new Lead
                    {
                        Id = Guid.NewGuid(),
                        LeadId = request.LeadId.Value,
                        IsActive = true,
                    };

                    await _context.Leads.AddAsync(lead, cancellationToken);
                }
            }

        }
        // trường hợp ANONYMOUS, người dùng ẩn danh
        else
        {
            if(!request.LeadId.HasValue)
                throw GrpcExceptions.InvalidArgument("LeadId must be provided for anonymous user.");

            lead = await _context.Leads
                  .FirstOrDefaultAsync(x => x.LeadId == request.LeadId, cancellationToken);

            if(lead == null)
            {
                lead = new Lead
                {
                    Id = Guid.NewGuid(),
                    LeadId = request.LeadId.Value,
                    IsActive = true,
                };

                await _context.Leads.AddAsync(lead, cancellationToken);
            }
        }

        // tạo Quiz Attempt

        var quizAttempt = new QuizAttempt
        {
            CampaignQuizSettingId = request.CampaignQuizSettingId,
            LeadId = lead?.Id,
            UserId = user?.Id,
            AttemptStatus = AttemptStatus.InProgress,
            StartedAt = DateTime.UtcNow
        };

        var inProgress = await _context.QuizAttempts
         .FirstOrDefaultAsync(x =>
             x.CampaignQuizSettingId == request.CampaignQuizSettingId &&
             (request.UserId != null
                 ? x.UserId == user!.Id
                 : x.LeadId == lead!.Id) &&
             x.AttemptStatus == AttemptStatus.InProgress,
             cancellationToken);

        if (inProgress != null)
            return _mapper.Map<QuizAttemptMiniResponseDto>(inProgress);

        await _context.QuizAttempts.AddAsync(quizAttempt, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<QuizAttemptMiniResponseDto>(quizAttempt);

        response.LeadId = request.LeadId;

        response.UserId = request.UserId;

        await transaction.CommitAsync(cancellationToken);
        return response;
    }

    public async Task<QuizResultMiniResponseDto> SubmitQuizAttempt(SubmitQuizAttemptCommand request, CancellationToken cancellationToken)
    {
        using var transaction = _context.Database.BeginTransaction();

        var attempt = await _context.QuizAttempts
            .Include(x => x.QuizResult)
            .FirstOrDefaultAsync(x => x.Id == request.QuizAttemptId, cancellationToken);

        if(attempt == null)
            throw GrpcExceptions.NotFound("QuizAttempt", request.QuizAttemptId);

        if(attempt.AttemptStatus != AttemptStatus.InProgress)
            throw GrpcExceptions.InvalidArgument("Quiz attempt has already been completed.");

        var campaignQuizSetting = await _context.CampaignQuizSettings
            .Include(x => x.Quiz)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                        .ThenInclude(x => x.AnswerDimensionScores)
            .FirstOrDefaultAsync(x => x.Id == attempt.CampaignQuizSettingId, cancellationToken);

        if(campaignQuizSetting == null || campaignQuizSetting.IsActive == false )
            throw GrpcExceptions.BadRequest("CampaignQuizSetting is not active or does not exist.");

        //var quiz = campaignQuizSetting.Quiz;
        var quiz = await GetEffectiveQuiz(campaignQuizSetting.QuizId, cancellationToken);

        ValidateAnswers(request.UserAnswers, quiz, 0);

        var (totalScore, dimensionScoreResults) = CalculateScore(request.UserAnswers, quiz);

        var result = new QuizResult
        {
            Id = Guid.NewGuid(),
            QuizAttemptId = attempt.Id,
            TotalScore = totalScore,
            DimensionScoreResults = dimensionScoreResults
        };

        _context.QuizResults.Add(result);

        attempt.AttemptStatus = AttemptStatus.Completed;
        attempt.CompletedAt = DateTime.UtcNow;
        attempt.UserAnswers = _mapper.Map<List<UserAnswer>>(request.UserAnswers);
        attempt.CurrentQuestionId = quiz.Questions.LastOrDefault()?.Id;

        var response = _mapper.Map<QuizResultMiniResponseDto>(result);

        var scoreLookup = dimensionScoreResults
            .ToDictionary(x => x.DimensionId, x => x.Score);

        var dimensions = await _context.Dimensions
            .Where(x => scoreLookup.Keys.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var dimensionResults = dimensions
            .Select(d => new DimensionScoreResultDto
            {
                DimensionId = d.Id,
                DimensionTitle = d.Title,
                Score = scoreLookup[d.Id]
            })
            .ToList();

        response.DimensionScoreResults = dimensionResults;

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return response;
    }

    private async Task<Domain.Entities.Quiz> GetEffectiveQuiz(Guid quizId, CancellationToken cancellationToken)
    {
        var quiz = await _context.Quizzes.FindAsync([quizId], cancellationToken);

        if (quiz == null)
            throw GrpcExceptions.NotFound("Quiz", quizId);

        if (quiz.Source == QuizSource.Refference && quiz.ParentId != null)
            quizId = quiz.ParentId.Value;

        var effectiveQuiz = await _context.Quizzes
            .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                    .ThenInclude(x => x.AnswerDimensionScores)
            .FirstOrDefaultAsync(x => x.Id == quizId, cancellationToken);

        if (effectiveQuiz == null)
            throw GrpcExceptions.NotFound("Effective Quiz", quizId);

        Console.WriteLine($"[Repo-Debug] GetEffectiveQuiz Loaded. Id: {effectiveQuiz.Id}, Source: {effectiveQuiz.Source}, ParentId: {effectiveQuiz.ParentId}, QuestionsCount: {effectiveQuiz.Questions.Count}");
        
        return effectiveQuiz;
    }

    private async Task MergeLeadToUserAsync(User user, Lead lead, CancellationToken cancellationToken)
    {
        // Nếu Lead đã có User liên kết rồi thì không merge nữa
        if(lead.LinkedUserId != null && lead.LinkedUserId != user.UserId)
            return;

        //Gán lead cho user
        if(lead.LinkedUserId == null)
        {
            lead.LinkedUserId = user.Id;
            lead.LinkedUser = user;
        }
        
        await _context.QuizAttempts.
            Where(x => x.LeadId == lead.Id && x.UserId == null)
            .ExecuteUpdateAsync(s => s.SetProperty(a => a.UserId, user.Id),
            cancellationToken);
    }

    // type == 0 -> validate khi submit quiz attempt, require phải trả lời tất cả câu hỏi
    // type == 1 -> validate khi update quiz attempt, không require phải trả lời tất cả câu hỏi, nhưng nếu trả lời thì phải đúng câu hỏi và câu trả lời thuộc quiz đó
    private static void ValidateAnswers(List<UserAnswerRequestDto> userAnswers, Domain.Entities.Quiz quiz, int type = 1, Guid? currentQuestionId = null)
    {
        if(type == 0)
        {
            if (userAnswers.Count != quiz.Questions.Count)
                throw GrpcExceptions.InvalidArgument(
                    "Number of answers does not match number of questions in the quiz.");

            var questionIdsFromUser = userAnswers.Select(x => x.QuestionId).ToHashSet();

            if (questionIdsFromUser.Count != quiz.Questions.Count)
                throw GrpcExceptions.InvalidArgument("Each question must be answered exactly once.");
        }

        var questionDict = quiz.Questions.ToDictionary(
            q => q.Id,
            q => new
            {
                Question = q,
                AnswerIds = q.Answers.Select(a => a.Id).ToHashSet()
            });

        foreach (var input in userAnswers)
        {
            if (!questionDict.TryGetValue(input.QuestionId, out var questionData))
            {
                var allQuestionIds = string.Join(", ", quiz.Questions.Select(q => q.Id.ToString()));
                Console.WriteLine($"[Repo-Debug] Validation FAILED! QuizId: {quiz.Id} (Parent: {quiz.ParentId}). Total Questions: {quiz.Questions.Count}. QuestionId '{input.QuestionId}' NOT FOUND in [{allQuestionIds}]");
                throw GrpcExceptions.InvalidArgument($"QuestionId {input.QuestionId} does not belong to the quiz.");
            }

            if (!questionData.AnswerIds.Contains(input.AnswerId))
                throw GrpcExceptions.InvalidArgument($"AnswerId {input.AnswerId} does not belong to QuestionId {input.QuestionId}.");
        }

        if(type == 1 && currentQuestionId != null)
        {
            if (!questionDict.ContainsKey(currentQuestionId.Value))
                throw GrpcExceptions.InvalidArgument($"CurrentQuestionId {currentQuestionId.Value} does not belong to the quiz.");
        }
    }

    private static (decimal total, List<DimensionScoreResult> dimensionScoreResults) CalculateScore (List<UserAnswerRequestDto> userAnswers, Domain.Entities.Quiz quiz)
    {
        decimal totalScore = 0;

        var dimensionDict = new Dictionary<Guid, decimal>();

        foreach(var input in userAnswers)
        {
            var question = quiz.Questions
                .First(q => q.Id == input.QuestionId);

            var answer = question.Answers
                .First(a => a.Id == input.AnswerId);

            // Grade (Tính điểm)
            if(question.Type == QuizType.Graded)
            {
                if(answer.IsCorrect == true)
                    totalScore += answer.Score;
            }

            // Dimension (Tính điểm theo từng Dimension)

            else if(question.Type == QuizType.Dimension)
            {
                if(answer.AnswerDimensionScores != null)
                {
                    foreach(var dim in answer.AnswerDimensionScores)
                    {
                        if (!dimensionDict.ContainsKey(dim.DimensionId))
                            dimensionDict[dim.DimensionId] = 0;

                        dimensionDict[dim.DimensionId] += dim.Score;
                    }
                }
            }
        }

        var dimensionScoreResults = dimensionDict
            .Select(x => new DimensionScoreResult
            {
                DimensionId = x.Key,
                Score = x.Value
            })
            .ToList();

        return (totalScore, dimensionScoreResults);
    }

    public async Task<QuizAttemptMiniResponseDto> UpdateQuizAttempt(UpdateQuizAttemptCommand request, CancellationToken cancellationToken)
    {
        var attempt = await _context.QuizAttempts.FindAsync([request.Id], cancellationToken);
        
        if(attempt == null)
            throw GrpcExceptions.NotFound("QuizAttempt", request.Id);

        if(attempt.AttemptStatus != AttemptStatus.InProgress)
            throw GrpcExceptions.InvalidArgument("Only quiz attempts that are in progress can be updated.");

        var campaignQuizSetting = await _context.CampaignQuizSettings
            .Include(x => x.Quiz)
                .ThenInclude(x => x.Questions)
                    .ThenInclude(x => x.Answers)
                        .ThenInclude(x => x.AnswerDimensionScores)
            .FirstOrDefaultAsync(x => x.Id == attempt.CampaignQuizSettingId, cancellationToken);

        if (campaignQuizSetting == null || campaignQuizSetting.IsActive == false)
            throw GrpcExceptions.BadRequest("CampaignQuizSetting is not active or does not exist.");

        var quiz = await GetEffectiveQuiz(campaignQuizSetting.QuizId, cancellationToken);

        ValidateAnswers(request.UserAnswers, quiz, 1, request.CurrentQuestionId);

        attempt.UserAnswers = _mapper.Map<List<UserAnswer>>(request.UserAnswers);
        attempt.CurrentQuestionId = request.CurrentQuestionId;

        var response = _mapper.Map<QuizAttemptMiniResponseDto>(attempt);

        await _context.SaveChangesAsync(cancellationToken);

        return response;
    }

    // lấy attempt đang làm giở
    public async Task<QuizAttemptResponseDto> GetQuizAttemptQuery(GetQuizAttemptQueryQuery request, CancellationToken cancellationToken)
    {
        if(request.UserId == null && request.LeadId == null)
            throw GrpcExceptions.InvalidArgument("Either UserId or LeadId must be provided.");

        QuizAttempt? attempt = null;

        if (request.LeadId != null)
        {
            attempt = await _context.QuizAttempts
              .Where(x =>
                  x.CampaignQuizSettingId == request.CampaignQuizSettingId &&
                  x.LeadId == request.LeadId &&
                  x.AttemptStatus == AttemptStatus.InProgress)
              .OrderByDescending(x => x.StartedAt)
              .FirstOrDefaultAsync(cancellationToken);
        }
        else if(request.UserId != null)
        {
            attempt = await _context.QuizAttempts
                .Where(x =>
                    x.CampaignQuizSettingId == request.CampaignQuizSettingId &&
                    x.UserId == request.UserId &&
                    x.AttemptStatus == AttemptStatus.InProgress)
                .OrderByDescending(x => x.StartedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        if (attempt == null)
            throw GrpcExceptions.NotFound($"No quiz attempt found for the specified criteria.");

        var response = _mapper.Map<QuizAttemptResponseDto>(attempt);

        return response;
    }
}

