namespace QuizSvc.Application.Commands.Interactions;

public class UpdateScoringRuleCommand : IRequest<ScoringRule>
{
    public string Tenant { get; set; } = default!;
    public string RuleKey { get; set; } = default!;
    public double Points { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; } = default!;
}

public class UpdateScoringRuleCommandHandler : IRequestHandler<UpdateScoringRuleCommand, ScoringRule>
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly IMapper _mapper;

    public UpdateScoringRuleCommandHandler(IInteractionRepository interactionRepository, IMapper mapper)
    {
        _interactionRepository = interactionRepository;
        _mapper = mapper;
    }

    public async Task<ScoringRule> Handle(UpdateScoringRuleCommand request, CancellationToken cancellationToken)
    {
        var rule = _mapper.Map<ScoringRule>(request);
        return await _interactionRepository.UpsertScoringRule(rule, cancellationToken);
    }
}
