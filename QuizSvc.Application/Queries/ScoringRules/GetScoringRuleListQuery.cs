using QuizSvc.Application.Dtos;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Queries.ScoringRules;

public class GetScoringRuleListQuery : IRequest<PagedList<ScoringRuleMiniResponseDto>>
{
    public string? RuleKey { get; set; }
    public bool? IsActive { get; set; }
    public required string Tenant { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}

public class GetScoringRuleListQueryHandler : IRequestHandler<GetScoringRuleListQuery, PagedList<ScoringRuleMiniResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IScoringRuleRepository _scoringRuleRepository;

    public GetScoringRuleListQueryHandler(
        IMapper mapper,
        IScoringRuleRepository scoringRuleRepository
    )
    {
        _mapper = mapper;
        _scoringRuleRepository = scoringRuleRepository;
    }
    public async Task<PagedList<ScoringRuleMiniResponseDto>> Handle(GetScoringRuleListQuery request, CancellationToken cancellationToken)
    {
        var result = await _scoringRuleRepository.GetScoringRuleList(request, cancellationToken);
        return result;
    }
}
