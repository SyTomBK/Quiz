using AutoMapper;
using MediatR;
using QuizSvc.Application.Contracts.Persistence;
using QuizSvc.Application.Dtos;
using QuizSvc.Domain.Entities;

namespace QuizSvc.Application.Queries.Interactions;

public class GetLeadEngagementQuery : IRequest<LeadEngagementResponseDto>
{
    public Guid LeadId { get; set; }
}

public class GetLeadEngagementQueryHandler : IRequestHandler<GetLeadEngagementQuery, LeadEngagementResponseDto>
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly ILeadRepository _leadRepository;
    private readonly IMapper _mapper;

    public GetLeadEngagementQueryHandler(IInteractionRepository interactionRepository, ILeadRepository leadRepository, IMapper mapper)
    {
        _interactionRepository = interactionRepository;
        _leadRepository = leadRepository;
        _mapper = mapper;
    }

    public async Task<LeadEngagementResponseDto> Handle(GetLeadEngagementQuery request, CancellationToken cancellationToken)
    {
        // 1. Resolve internal Lead.Id from external request.LeadId
        var lead = await _leadRepository.GetByLeadIdAsync(request.LeadId, cancellationToken);
        if (lead == null) return new LeadEngagementResponseDto { LeadId = request.LeadId, OverallEngagementScore = 0 };

        // 2. Fetch profile using internal lead.Id
        var profile = await _interactionRepository.GetLeadEngagementProfile(lead.Id, cancellationToken);
        if (profile == null) return new LeadEngagementResponseDto { LeadId = request.LeadId, OverallEngagementScore = 0 };

        var mapped = _mapper.Map<LeadEngagementResponseDto>(profile);
        mapped.LeadId = request.LeadId; // Ensure mapping uses external ID for response
        
        return mapped;
    }
}
