using QuizSvc.Application.Dtos;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Queries.Campains;
public class GetCampaignListQuery : IRequest<PagedList<CampaignMiniResponseDto>>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public CampaignStatus? Status { get; set; }
    public required string Tenant { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}

public class GetCampaignListQueryHandler : IRequestHandler<GetCampaignListQuery, PagedList<CampaignMiniResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly ICampaignRepository _campaignRepository;
    public GetCampaignListQueryHandler(IMapper mapper, ICampaignRepository campaignRepository)
    {
        _mapper = mapper;
        _campaignRepository = campaignRepository;
    }
    public async Task<PagedList<CampaignMiniResponseDto>> Handle(GetCampaignListQuery request, CancellationToken cancellationToken)
    {
        var result = await _campaignRepository.GetCampaignList(request, cancellationToken);
        return result;
    }
}
