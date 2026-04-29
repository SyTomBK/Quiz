using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Quizs;
using QuizSvc.Share.Utils;

namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ICodeGenerator _codeGenerator;
    public QuizRepository(DataContext context, IMapper mapper, ICodeGenerator codeGenerator)
    {
        _context = context;
        _mapper = mapper;
        _codeGenerator = codeGenerator;
    }

    public async Task EnsureQuizExist(Guid quizId, CancellationToken cancellationToken)
    {
        var exists = await _context.Quizzes.AnyAsync(q => q.Id == quizId, cancellationToken);

        if (!exists)
            throw GrpcExceptions.NotFound("Quiz", quizId);
    }
    
    public async Task<QuizResponseDto> GetQuizDetail(Guid id, CancellationToken cancellationToken)
    {
        var quiz = await _context.Quizzes.FindAsync([id], cancellationToken);

        if (quiz == null)
            throw GrpcExceptions.NotFound("Quiz", id);

        var contentSourceId = (quiz.Source == QuizSource.Refference && quiz.ParentId != null) 
                            ? quiz.ParentId : quiz.Id;

        var quizAttempts = _context.QuizAttempts;

        var contentData = await _context.Quizzes
            .AsNoTracking()
            .Include(x => x.Dimensions)
                .Include(x => x.Questions.OrderBy(q => q.Order))
                .ThenInclude(q => q.Answers)
                    .ThenInclude(a => a.AnswerDimensionScores)
                        .ThenInclude(ds => ds.Dimension)
            .FirstOrDefaultAsync(x => x.Id == contentSourceId, cancellationToken);

        var questions = contentData?.Questions.OrderBy(q => q.Order)
            .Select(q => new QuestionMiniResponseDto()
            {
                Id = q.Id,
                Content = q.Content,
                Type = q.Type,
                Order = q.Order,
                Answers = q.Answers.Select(a => new AnswerMiniResponseDto
                {
                    Id = a.Id,
                    Content = a.Content,
                    IsCorrect = a.IsCorrect,
                    Score = a.Score,
                    DimensionScores = a.AnswerDimensionScores.Select(ds => new DimensionScoreMiniResponseDto
                    {
                        Id = ds.Id,
                        DimensionId = ds.Dimension.Id,
                        Title = ds.Dimension.Title,
                        Score = ds.Score
                    }).ToList()
                }).ToList()
            }).ToList() ?? [];

        var response = new QuizResponseDto
        {
            Id = quiz.Id,
            Code = quiz.Code,
            Title = quiz.Title,
            Description = quiz.Description,
            Instruction = quiz.Instruction,
            Image = quiz.Image,
            Tenant = quiz.Tenant!,
            ParentId = quiz.ParentId,
            Type = quiz.Type,
            Source = quiz.Source,
            EstimateTime = quiz.EstimateTime,
            // Dimensions lấy từ contentData
            Dimensions = contentData?.Dimensions?.Select(d => new DimensionMiniResponseDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description
            }).ToList() ?? [],
            Questions = questions,
            CreatedAt = quiz.CreatedAt,
            CreatedBy = quiz.CreatedBy,
            LastModifiedAt = quiz.LastModifiedAt,
            LastModifiedBy = quiz.LastModifiedBy
        };
        return response;
    }

    public async Task<PagedList<QuizMiniResponseDto>> GetQuizList(GetQuizListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Quizzes
            .AsQueryable().AsNoTracking()
            .WhereIfNotEmpty(request.Code, x => EF.Functions.ILike(x.Code, $"%{request.Code}%"))
            .WhereIfNotEmpty(request.Tenant, x => EF.Functions.ILike(x.Tenant!, request.Tenant!))
            .WhereIfNotEmpty(request.Title, x => EF.Functions.ILike(x.Title, $"%{request.Title}%"))
            .WhereIfHasValue(request.Type, x => x.Type == request.Type!.Value)
            .WhereIfAny(request.Sources, x => request.Sources!.Contains(x.Source))
            .WhereIfNotEmpty(request.CampaignId,
                x => x.CampaignQuizSettings.Any(cqs => cqs.CampaignId == request.CampaignId!.Value)
            )
            .WhereIf(request.FromDate.HasValue, x => x.CreatedAt >= request.FromDate!.Value.Date)
            .WhereIf(request.ToDate.HasValue, x => x.CreatedAt <= request.ToDate!.Value.Date);

        var total = await query.CountAsync(cancellationToken);

        var quizAttempts = _context.QuizAttempts;

        var data = await query
         .OrderByDescending(x => x.CreatedAt)
         .Skip((request.Page - 1) * request.PageSize)
         .Take(request.PageSize)
         .Select(item => new QuizMiniResponseDto
         {
             Id = item.Id,
             Code = item.Code,
             Title = item.Title,
             Description = item.Description,
             Instruction = item.Instruction,
             Image = item.Image,
             Tenant = item.Tenant!,
             ParentId = item.ParentId,
             Type = item.Type,
             Source = item.Source,
             EstimateTime = item.EstimateTime,
             CreatedAt = item.CreatedAt,
             SubmitedAmount = quizAttempts
                .Where(qa =>
                    qa.QuizResult != null &&
                    qa.LeadId.HasValue && 
                    qa.CampaignQuizSetting.QuizId == item.Id &&
                    (!request.CampaignId.HasValue
                        || qa.CampaignQuizSetting.CampaignId == request.CampaignId.Value))
                .Select(qa => qa.LeadId!.Value)
                .Distinct()
                .Count()
         })
         .ToListAsync(cancellationToken);

        return PagedList<QuizMiniResponseDto>.Create(data, total, request.Page, request.PageSize);
    }

    public async Task<List<Guid>> GetExistingIds(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await _context.Quizzes
            .Where(x => ids.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entities.Quiz?> GetTemplateById(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Quizzes
            .FirstOrDefaultAsync(x => x.Id == id && x.Source == QuizSource.Template, cancellationToken);
    }

    public async Task AddAsync(Domain.Entities.Quiz quiz, CancellationToken cancellationToken)
    {
        await _context.Quizzes.AddAsync(quiz, cancellationToken);
    }

    public async Task<Domain.Entities.Quiz?> GetTemplateFullGraph(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Quizzes.AsNoTracking()
           .Include(x => x.Dimensions)
           .Include(x => x.Questions)
               .ThenInclude(q => q.Answers)
                   .ThenInclude(a => a.AnswerDimensionScores)
           .FirstOrDefaultAsync(x => x.Id == id && x.Source == QuizSource.Template, cancellationToken);
    }

    public async Task<GetQuizInCampaignDetailResponseDto> GetQuizInCampaignDetail(Guid quizId, Guid campaignId, CancellationToken cancellationToken)
    {
        var quiz = await _context.Quizzes.FindAsync([quizId], cancellationToken);

        if (quiz == null)
            throw GrpcExceptions.NotFound("Quiz", quizId);

        var contentSourceId = (quiz.Source == QuizSource.Refference && quiz.ParentId != null)
                            ? quiz.ParentId : quiz.Id;

        var quizAttempts = _context.QuizAttempts;

        var contentData = await _context.Quizzes
            .AsNoTracking()
            .Include(x => x.Dimensions)
                .Include(x => x.Questions.OrderBy(q => q.Order))
                .ThenInclude(q => q.Answers)
                    .ThenInclude(a => a.AnswerDimensionScores)
                        .ThenInclude(ds => ds.Dimension)
            .FirstOrDefaultAsync(x => x.Id == contentSourceId, cancellationToken);

        var questions = contentData?.Questions.OrderBy(q => q.Order)
            .Select(q => new QuestionMiniResponseDto()
            {
                Id = q.Id,
                Content = q.Content,
                Type = q.Type,
                Order = q.Order,
                Answers = q.Answers.Select(a => new AnswerMiniResponseDto
                {
                    Id = a.Id,
                    Content = a.Content,
                    IsCorrect = a.IsCorrect,
                    Score = a.Score,
                    DimensionScores = a.AnswerDimensionScores.Select(ds => new DimensionScoreMiniResponseDto
                    {
                        Id = ds.Id,
                        DimensionId = ds.Dimension.Id,
                        Title = ds.Dimension.Title,
                        Score = ds.Score
                    }).ToList()
                }).ToList()
            }).ToList() ?? [];

        var quizSetting = await _context.CampaignQuizSettings
            .FirstOrDefaultAsync(x => x.CampaignId == campaignId && x.QuizId == quizId, cancellationToken);

        if (quizSetting == null)
            throw GrpcExceptions.InvalidArgument("This quiz is not belong this campaign!");

        var response = new GetQuizInCampaignDetailResponseDto
        {
            Id = quiz.Id,
            Code = quiz.Code,
            Title = quiz.Title,
            Description = quiz.Description,
            Instruction = quiz.Instruction,
            Image = quiz.Image,
            Tenant = quiz.Tenant!,
            ParentId = quiz.ParentId,
            Type = quiz.Type,
            Source = quiz.Source,
            EstimateTime = quiz.EstimateTime,
            SubmitedAmount = await quizAttempts
                .Where(qa => qa.QuizResult != null && qa.LeadId.HasValue &&
                    qa.CampaignQuizSetting.QuizId == quizId && qa.CampaignQuizSetting.CampaignId == campaignId)
                .Select(qa => qa.LeadId!.Value)
                .Distinct()
                .CountAsync(cancellationToken),
            // Dimensions lấy từ contentData
            Dimensions = contentData?.Dimensions?.Select(d => new DimensionMiniResponseDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description
            }).ToList() ?? [],
            QuizSetting = _mapper.Map<CampaignQuizSettingResponseDto>(quizSetting),
            Questions = questions,
            CreatedAt = quiz.CreatedAt,
            CreatedBy = quiz.CreatedBy,
            LastModifiedAt = quiz.LastModifiedAt,
            LastModifiedBy = quiz.LastModifiedBy
        };

        return response;
    }

    public async Task<Domain.Entities.Quiz?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Quizzes
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Domain.Entities.Quiz?> GetWithQuestionAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Quizzes.AsNoTracking()
            .Include(q => q.Questions)
                .ThenInclude(x => x.Answers)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

}