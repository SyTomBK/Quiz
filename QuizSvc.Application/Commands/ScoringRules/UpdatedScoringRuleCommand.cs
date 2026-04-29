using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.ScoringRules;

public class UpdatedScoringRuleCommand : IRequest<ScoringRuleMiniResponseDto>
{
    public Guid Id { get; set; }
    public required string Tenant { get; set; }
    public required string RuleKey { get; set; }
    public string? Description { get; set; }
    public double Points { get; set; }
    public bool IsActive { get; set; }
}

public class UpdatedScoringRuleCommandHandler : IRequestHandler<UpdatedScoringRuleCommand, ScoringRuleMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IScoringRuleRepository _scoringRuleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatedScoringRuleCommandHandler(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IScoringRuleRepository scoringRuleRepository
    )
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _scoringRuleRepository = scoringRuleRepository;
    }
    public async Task<ScoringRuleMiniResponseDto> Handle(UpdatedScoringRuleCommand request, CancellationToken cancellationToken)
    {
        var scoringRule = await _scoringRuleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (scoringRule == null)
            throw GrpcExceptions.NotFound("ScoringRule", request.Id);

        if(request.Tenant != scoringRule.Tenant || request.RuleKey != scoringRule.RuleKey)
        {
            var exist = await _scoringRuleRepository.ExistByRuleKeyAndTenant(request.RuleKey, request.Tenant, cancellationToken);
            if(exist)
                throw GrpcExceptions.Conflict($"ScoringRule with RuleKey '{request.RuleKey}' already exists for Tenant '{request.Tenant}'.");
        }
        
        _mapper.Map(request, scoringRule);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ScoringRuleMiniResponseDto>(scoringRule);
    }
}
