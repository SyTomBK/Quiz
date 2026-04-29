using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.QuizAttempts;

public class SubmitQuizAttemptCommand : IRequest<QuizResultMiniResponseDto>
{
    public Guid QuizAttemptId { get; set; }
    public List<UserAnswerRequestDto> UserAnswers { get; set; } = new();
}
public class SubmitQuizAttemptCommandHandler : IRequestHandler<SubmitQuizAttemptCommand, QuizResultMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizAttemptRepository _quizAttemptRepository;
    public SubmitQuizAttemptCommandHandler(IMapper mapper, IQuizAttemptRepository quizAttemptRepository)
    {
        _mapper = mapper;
        _quizAttemptRepository = quizAttemptRepository;
    }

    public async Task<QuizResultMiniResponseDto> Handle(SubmitQuizAttemptCommand request, CancellationToken cancellationToken)
    {
        var response = await _quizAttemptRepository.SubmitQuizAttempt(request, cancellationToken);
        return response;
    }
}
     