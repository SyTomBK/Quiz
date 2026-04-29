using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.Interactions;

public class GetScoringRulesQuery : IRequest<List<ScoringRuleDto>>
{
    public string Tenant { get; set; } = default!;
}

public class GetScoringRulesQueryHandler : IRequestHandler<GetScoringRulesQuery, List<ScoringRuleDto>>
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly IMapper _mapper;

    public GetScoringRulesQueryHandler(IInteractionRepository interactionRepository, IMapper mapper)
    {
        _interactionRepository = interactionRepository;
        _mapper = mapper;
    }

    public async Task<List<ScoringRuleDto>> Handle(GetScoringRulesQuery request, CancellationToken cancellationToken)
    {
        var rules = await _interactionRepository.GetScoringRules(request.Tenant, cancellationToken);

        return _mapper.Map<List<ScoringRuleDto>>(rules);
    }
}
