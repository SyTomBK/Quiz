using QuizSvc.Application.Dtos;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Queries.Leads;
public class GetLeadListQuery : IRequest<PagedList<LeadMiniResponseDto>>
{
    public string? Key { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? ReferralCode { get; set; }
    public string? Tenant { get; set; }
    public Guid? CustomerId { get; set; }
    public bool SortByScore { get; set; }
    public bool IsTopLeads { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}

public class GetLeadListQueryHandler : IRequestHandler<GetLeadListQuery, PagedList<LeadMiniResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly ILeadRepository _leadRepository;
    public GetLeadListQueryHandler(IMapper mapper, ILeadRepository leadRepository)
    {
        _mapper = mapper;
        _leadRepository = leadRepository;
    }
    public async Task<PagedList<LeadMiniResponseDto>> Handle(GetLeadListQuery request, CancellationToken cancellationToken)
    {
        var result = await _leadRepository.GetLeadList(request, cancellationToken);
        return result;
    }
}
