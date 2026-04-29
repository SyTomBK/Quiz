using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.QuizAttempts;

public class GetQuizAttemptQueryQuery : IRequest<QuizAttemptResponseDto>
{
    public Guid CampaignQuizSettingId { get; set; }
    public Guid? LeadId { get; set; }
    public Guid? UserId { get; set; }
}
public class GetQuizAttemptQueryQueryHandler : IRequestHandler<GetQuizAttemptQueryQuery, QuizAttemptResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizAttemptRepository _quizAttemptRepository;
    public GetQuizAttemptQueryQueryHandler(IMapper mapper, IQuizAttemptRepository quizAttemptRepository)
    {
        _mapper = mapper;
        _quizAttemptRepository = quizAttemptRepository;
    }
    public async Task<QuizAttemptResponseDto> Handle(GetQuizAttemptQueryQuery request, CancellationToken cancellationToken)
    {
        var result = await _quizAttemptRepository.GetQuizAttemptQuery(request, cancellationToken);
        return result;
    }
}
