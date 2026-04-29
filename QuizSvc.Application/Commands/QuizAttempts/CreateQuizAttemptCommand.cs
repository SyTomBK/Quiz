using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.QuizAttempts;

public class CreateQuizAttemptCommand : IRequest<QuizAttemptMiniResponseDto>
{
    public Guid CampaignQuizSettingId { get; set; }
    public Guid? LeadId { get; set; }
    public Guid? UserId { get; set; }
    public string? Username { get; set; }
    public string? Tenant { get; set; }
}

public class CreateQuizAttemptCommandHandler : IRequestHandler<CreateQuizAttemptCommand, QuizAttemptMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizAttemptRepository _quizAttemptRepository;
    public CreateQuizAttemptCommandHandler(IMapper mapper, IQuizAttemptRepository quizAttemptRepository)
    {
        _mapper = mapper;
        _quizAttemptRepository = quizAttemptRepository;
    }
    public async Task<QuizAttemptMiniResponseDto> Handle(CreateQuizAttemptCommand request, CancellationToken cancellationToken)
    {
        var result = await _quizAttemptRepository.CreateQuizAttempt(request, cancellationToken);
        return result;
    }
}
