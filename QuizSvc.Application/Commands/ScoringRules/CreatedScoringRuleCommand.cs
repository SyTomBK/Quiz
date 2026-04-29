using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.ScoringRules;

public class CreatedScoringRuleCommand : IRequest<ScoringRuleMiniResponseDto>
{
    public required string Tenant { get; set; }
    public required string RuleKey { get; set; }
    public string? Description { get; set; }
    public double Points { get; set; }
    public bool IsActive { get; set; }
}

public class CreatedScoringRuleCommandHandler : IRequestHandler<CreatedScoringRuleCommand, ScoringRuleMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IScoringRuleRepository _scoringRuleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatedScoringRuleCommandHandler(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IScoringRuleRepository scoringRuleRepository
    )
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _scoringRuleRepository = scoringRuleRepository;
    }

    public async Task<ScoringRuleMiniResponseDto> Handle(CreatedScoringRuleCommand request, CancellationToken cancellationToken)
    {
        var exist = await _scoringRuleRepository.ExistByRuleKeyAndTenant(request.RuleKey, request.Tenant, cancellationToken);
        
        if(exist) 
            throw GrpcExceptions.AlreadyExists("Scoring rule with the same rule key already exists for the tenant.");

        var scoringRule = _mapper.Map<ScoringRule>(request);

        await _scoringRuleRepository.AddAsync(scoringRule, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ScoringRuleMiniResponseDto>(scoringRule);
    }
}
