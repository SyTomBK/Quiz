using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.CampaignQuizSettings;

public class GetCampaignQuizSettingDetailQuery : IRequest<CampaignQuizSettingResponseDto>
{
    public Guid Id { get; set; }
}
public class GetCampaignQuizSettingDetailQueryHandler : IRequestHandler<GetCampaignQuizSettingDetailQuery, CampaignQuizSettingResponseDto>
{
    private readonly ICampaignQuizSettingRepository _campaignQuizSettingRepository;
    private readonly IMapper _mapper;
    public GetCampaignQuizSettingDetailQueryHandler(
        IMapper mapper,
       ICampaignQuizSettingRepository campaignQuizSettingRepository
    )
    {
        _mapper = mapper;
        _campaignQuizSettingRepository = campaignQuizSettingRepository;
    }
    public async Task<CampaignQuizSettingResponseDto> Handle(GetCampaignQuizSettingDetailQuery request, CancellationToken cancellationToken)
    {
        var setting = await _campaignQuizSettingRepository.GetByIdAsync(request.Id, cancellationToken);

        return _mapper.Map<CampaignQuizSettingResponseDto>(setting);
    }
}
