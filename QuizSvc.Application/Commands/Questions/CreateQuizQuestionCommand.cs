using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Contracts.Persistence;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.Questions;

public class CreateQuizQuestionCommand : IRequest<QuestionSuccessResponseDto>
{
    public required string Content { get; set; }
    public required Guid QuizId { get; set; }
    public List<CreatedQuizAnswerRequestDto> Answers { get; set; } = [];
}

public class CreateQuizQuestionCommandHandler : IRequestHandler<CreateQuizQuestionCommand, QuestionSuccessResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuestionRepository _questionRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IDimensionRepository _dimensionRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateQuizQuestionCommandHandler(IMapper mapper, IQuestionRepository questionRepository,
        IUnitOfWork unitOfWork, IDimensionRepository dimensionRepository,
        IQuizRepository quizRepository)
    {
        _mapper = mapper;
        _questionRepository = questionRepository;
        _quizRepository = quizRepository;
        _dimensionRepository = dimensionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<QuestionSuccessResponseDto> Handle(CreateQuizQuestionCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetWithQuestionAsync(request.QuizId, cancellationToken);

        if (quiz == null)
            throw GrpcExceptions.NotFound("Quiz", request.QuizId);

        var order = quiz.Questions.Max(x => x.Order) + 1;

        // validate DimensionId
        if (quiz.Type == QuizType.Dimension)
        {
            var validDimensionIds = await _dimensionRepository.GetDimensionIdsByQuizId(request.QuizId, cancellationToken);

            var requestedDimensionIds = request.Answers
                .SelectMany(a => a.DimensionScores)
                .Select(ds => ds.DimensionId)
                .Distinct()
                .ToList();

            if (requestedDimensionIds.Any(id => !validDimensionIds.Contains(id)))
                throw GrpcExceptions.BadRequest("Một số DimensionId không tồn tại hoặc không thuộc bài Quiz này.");
        }

        var question = new Question
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            Type = quiz.Type,
            QuizId = request.QuizId,
            Order = order,
            Answers = request.Answers.Select(item => new Answer
            {
                Content = item.Content,
                IsCorrect = item.IsCorrect,
                Score = item.Score,
                AnswerDimensionScores = quiz.Type == QuizType.Dimension
                ? item.DimensionScores.Select(ds => new AnswerDimensionScore
                {
                    DimensionId = ds.DimensionId,
                    Score = ds.Score
                }).ToList()
                : []
            }).ToList()
        };

        await _questionRepository.AddAsync(question, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<QuestionSuccessResponseDto>(question);
    }
}
