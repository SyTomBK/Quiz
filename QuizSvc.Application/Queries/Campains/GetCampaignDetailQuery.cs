using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.Campains;
public class GetCampaignDetailQuery : IRequest<CampaignResponseDto>
{
    public Guid Id { get; set; }
}

public class GetCampaignDetailQueryHandler : IRequestHandler<GetCampaignDetailQuery, CampaignResponseDto>
{
    private readonly IMapper _mapper;
    private readonly ICampaignRepository _campaignRepository;
    public GetCampaignDetailQueryHandler(IMapper mapper, ICampaignRepository campaignRepository)
    {
        _mapper = mapper;
        _campaignRepository = campaignRepository;
    }
    public async Task<CampaignResponseDto> Handle(GetCampaignDetailQuery request, CancellationToken cancellationToken)
    {
        var result = await _campaignRepository.GetCampaignDetail(request.Id, cancellationToken);
        return result;
    }
}
