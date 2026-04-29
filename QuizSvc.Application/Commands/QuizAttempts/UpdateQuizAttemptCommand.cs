using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.QuizAttempts;
public class UpdateQuizAttemptCommand : IRequest<QuizAttemptMiniResponseDto>
{
    public Guid Id { get; set; }
    public Guid CurrentQuestionId { get; set; }
    public List<UserAnswerRequestDto> UserAnswers { get; set; } = new();
}
public class UpdateQuizAttemptCommandHandler : IRequestHandler<UpdateQuizAttemptCommand, QuizAttemptMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizAttemptRepository _quizAttemptRepository;
    public UpdateQuizAttemptCommandHandler(IMapper mapper, IQuizAttemptRepository quizAttemptRepository)
    {
        _mapper = mapper;
        _quizAttemptRepository = quizAttemptRepository;
    }
    public async Task<QuizAttemptMiniResponseDto> Handle(UpdateQuizAttemptCommand request, CancellationToken cancellationToken)
    {
        var result = await _quizAttemptRepository.UpdateQuizAttempt(request, cancellationToken);
        return result;
    }
}
