using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.Leads;
public class GetLeadDetailQuery : IRequest<LeadResponseDto>
{
    public Guid LeadId { get; set; }    
}

public class GetLeadDetailQueryHandler : IRequestHandler<GetLeadDetailQuery, LeadResponseDto>
{
    private readonly IMapper _mapper;
    private readonly ILeadRepository _leadRepository;
    public GetLeadDetailQueryHandler(IMapper mapper, ILeadRepository leadRepository)
    {
        _mapper = mapper;
        _leadRepository = leadRepository;
    }
    public async Task<LeadResponseDto> Handle(GetLeadDetailQuery request, CancellationToken cancellationToken)
    {
        var lead = await _leadRepository.GetByLeadIdAsync(request.LeadId, cancellationToken);

        if (lead == null)
        {
            throw GrpcExceptions.NotFound($"Lead with leadId = {request.LeadId} not found");
        }

        return _mapper.Map<LeadResponseDto>(lead);
    }
}
