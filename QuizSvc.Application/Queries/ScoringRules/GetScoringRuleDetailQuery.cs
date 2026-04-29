using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.ScoringRules;

public class GetScoringRuleDetailQuery : IRequest<ScoringRuleDetailResponseDto>
{
    public Guid Id { get; set; }
}
public class GetScoringRuleDetailQueryHandler : IRequestHandler<GetScoringRuleDetailQuery, ScoringRuleDetailResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IScoringRuleRepository _scoringRuleRepository;

    public GetScoringRuleDetailQueryHandler(
        IMapper mapper,
        IScoringRuleRepository scoringRuleRepository
    )
    {
        _mapper = mapper;
        _scoringRuleRepository = scoringRuleRepository;
    }
    public async Task<ScoringRuleDetailResponseDto> Handle(GetScoringRuleDetailQuery request, CancellationToken cancellationToken)
    {
        var scoringRule = await _scoringRuleRepository.GetByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<ScoringRuleDetailResponseDto>(scoringRule);
    }
}
