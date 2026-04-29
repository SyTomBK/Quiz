using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.Interactions;

public class GetLeadInteractionsQuery : IRequest<PagedInteractionLogMiniResponseDto>
{
    public Guid LeadId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetLeadInteractionsQueryHandler : IRequestHandler<GetLeadInteractionsQuery, PagedInteractionLogMiniResponseDto>
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly IMapper _mapper;

    public GetLeadInteractionsQueryHandler(IInteractionRepository interactionRepository, IMapper mapper)
    {
        _interactionRepository = interactionRepository;
        _mapper = mapper;
    }

    public async Task<PagedInteractionLogMiniResponseDto> Handle(GetLeadInteractionsQuery request, CancellationToken cancellationToken)
    {
        var result = await _interactionRepository.GetLeadInteractions(request.LeadId, request.Page, request.PageSize, cancellationToken);
        
        var dtos = _mapper.Map<List<InteractionLogMiniResponseDto>>(result.data);
        
        if (dtos.Any())
        {
            var tenantId = dtos.First().Tenant;
            var rules = await _interactionRepository.GetScoringRules(tenantId, cancellationToken);
            var ruleDict = rules.ToDictionary(r => r.RuleKey, r => r.Points);
            
            foreach (var dto in dtos)
            {
                if (ruleDict.TryGetValue(dto.EventType, out var points))
                {
                    dto.Score = dto.Value * points;
                }
            }
        }
        
        return new PagedInteractionLogMiniResponseDto
        {
            Data = dtos,
            TotalCount = result.totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
