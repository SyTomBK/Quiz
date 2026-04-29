using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.Questions;

public class UpdateQuestionCommand : IRequest<QuestionSuccessResponseDto>
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public required int Order { get; set; }
    public required List<UpdatedAnswerRequestDto> Answers { get; set; } = new List<UpdatedAnswerRequestDto>();
}

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, QuestionSuccessResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAnswerRepository _answerRepository;
    private readonly IDimensionRepository _dimensionRepository;
    private readonly IAnswerDimensionScoresRepository _answerDimensionScoresRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateQuestionCommandHandler(IMapper mapper,
        IDimensionRepository dimensionRepository, IAnswerRepository answerRepository,
        IAnswerDimensionScoresRepository answerDimensionScoresRepository,
        IQuestionRepository questionRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _questionRepository = questionRepository;
        _dimensionRepository = dimensionRepository;
        _answerRepository = answerRepository;
        _answerDimensionScoresRepository = answerDimensionScoresRepository;
    }
    public async Task<QuestionSuccessResponseDto> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var question = await _questionRepository.GetWithIncludeByIdAsync(request.Id, cancellationToken);

        if(question == null)
            throw GrpcExceptions.NotFound("Question", request.Id);

        if (question.Type == QuizType.Dimension)
        {
            var validDimensionIds = await _dimensionRepository.GetDimensionIdsByQuizId(question.QuizId, cancellationToken);

            var requestedDimensionIds = request.Answers
                .SelectMany(a => a.DimensionScores)
                .Select(ds => ds.DimensionId)
                .Distinct()
                .ToList();

            if (requestedDimensionIds.Any(id => !validDimensionIds.Contains(id)))
                throw GrpcExceptions.BadRequest("Một số DimensionId không tồn tại hoặc không thuộc bài Quiz này.");
        }

        question.Content = request.Content;
        question.Order = request.Order;

        //1. Update existing answers and their dimension scores
        var answerIdsInRequest = request.Answers.Where(a => a.Id != Guid.Empty).Select(a => a.Id).ToList();

        // xóa những Answer không còn trong danh sách gửi lên
        var answersToRemove = question.Answers
            .Where(a => !answerIdsInRequest.Contains(a.Id))
            .Select(a => a.Id)
            .ToList();

        await _answerRepository.DeleteByIdsAsync(answersToRemove, cancellationToken);

        foreach (var item in request.Answers)
        {
            if (item.Id != Guid.Empty && question.Answers.Any(x => x.Id == item.Id))
            {
                // Update Answer hiện có
                var existingAnswer = question.Answers.First(a => a.Id == item.Id);
                existingAnswer.Content = item.Content;
                existingAnswer.IsCorrect = item.IsCorrect;
                existingAnswer.Score = item.Score;

                // Cập nhật AnswerDimensionScores
                if (question.Type == QuizType.Dimension)
                {
                    await _answerDimensionScoresRepository.DeleteByAnswerIdAsync(existingAnswer.Id, cancellationToken);

                    var entities = item.DimensionScores.Select(ds => new AnswerDimensionScore
                    {
                        AnswerId = existingAnswer.Id,
                        DimensionId = ds.DimensionId,
                        Score = ds.Score
                    }).ToList();

                    await _answerDimensionScoresRepository.AddRangeAsync(entities, cancellationToken);
                }
            }
            else
            {
                // Thêm Answer mới
                var newAnswer = new Answer
                {
                    Content = item.Content,
                    IsCorrect = item.IsCorrect,
                    Score = item.Score,
                    QuestionId = question.Id,
                    AnswerDimensionScores = question.Type == QuizType.Dimension ? item.DimensionScores.Select(ds => new AnswerDimensionScore
                    {
                        DimensionId = ds.DimensionId,
                        Score = ds.Score
                    }).ToList() : []
                };
                question.Answers.Add(newAnswer);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<QuestionSuccessResponseDto>(question);
    }
}
