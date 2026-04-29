using QuizSvc.Application.Commands.Quizs;
using QuizSvc.Application.Services.Questions;

namespace QuizSvc.Application.Services.Quizes;

public class QuizService : IQuizService
{
    private readonly ICodeGenerator _codeGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQuizRepository _quizRepository;
    private readonly IQuestionService _questionService;
    public QuizService(ICodeGenerator codeGenerator,
        IQuestionService questionService,
        IUnitOfWork unitOfWork, IQuizRepository quizRepository)
    {
        _codeGenerator = codeGenerator;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
        _questionService = questionService;
    }

    public async Task<Domain.Entities.Quiz> CreateQuiz(CreatedQuizCommand request,  CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var dimensionIdMap = request.Dimensions?.ToDictionary(
           d => d.TempDimId,
           d => Guid.NewGuid()
       ) ?? new Dictionary<string, Guid>();

        var dimensions = request.Dimensions?.Select(d => new Dimension
        {
            Id = dimensionIdMap[d.TempDimId],
            Title = d.Title,
            Description = d.Description
        }).ToList() ?? new List<Dimension>();

        var questions = _questionService.Build( request.Questions, request.Type, dimensionIdMap);

        var quiz = new Domain.Entities.Quiz
        {
            Code = _codeGenerator.Generate("QIZ"),
            Title = request.Title,
            Description = request.Description,
            Instruction = request.Instruction,
            Image = request.Image,
            Type = request.Type,
            Tenant = request.Tenant,
            Source = request.Source,
            EstimateTime = request.EstimateTime,
            Dimensions = request.Type == QuizType.Dimension ? dimensions : null,
            Questions = questions
        };

        await _quizRepository.AddAsync(quiz, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return quiz;
    }

    public Domain.Entities.Quiz DeepCopy(Domain.Entities.Quiz template, DeepCopyQuizCommand request)
    {
        var newQuiz = new Domain.Entities.Quiz
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Instruction = request.Instruction,
            Image = request.Image,
            Tenant = request.Tenant,
            EstimateTime = request.EstimateTime,
            Source = QuizSource.DeepCopy,
            Type = template.Type,
            ParentId = template.Id,
            Code = _codeGenerator.Generate("QIZ"),
        };

        var dimensionMap = new Dictionary<Guid, Guid>();

        newQuiz.Dimensions = template.Dimensions?.Select(d =>
        {
            var newId = Guid.NewGuid();
            dimensionMap[d.Id] = newId;

            return new Dimension
            {
                Id = newId,
                Title = d.Title,
                Description = d.Description,
                Quiz = newQuiz
            };
        }).ToList() ?? [];

        newQuiz.Questions = template.Questions?.Select(q => new Question
        {
            Id = Guid.NewGuid(),
            Content = q.Content,
            Type = q.Type,
            Order = q.Order,
            Quiz = newQuiz,

            Answers = q.Answers.Select(a => new Answer
            {
                Id = Guid.NewGuid(),
                Content = a.Content,
                IsCorrect = a.IsCorrect,
                Score = a.Score,

                AnswerDimensionScores = (q.Type == QuizType.Dimension && a.AnswerDimensionScores != null)
                    ? a.AnswerDimensionScores
                        .Where(ds => dimensionMap.ContainsKey(ds.DimensionId))
                        .Select(ds => new AnswerDimensionScore
                        {
                            Id = Guid.NewGuid(),
                            DimensionId = dimensionMap[ds.DimensionId],
                            Score = ds.Score
                        }).ToList()
                    : []
            }).ToList()
        }).ToList() ?? [];

        return newQuiz;
    }
}
