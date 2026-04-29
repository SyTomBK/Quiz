using QuizSvc.Application.Commands.Campaigns;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Campains;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Contracts.Persistence;

public interface ICampaignRepository
{
    Task AddAsync(Campaign campaign, CancellationToken cancellationToken);
    Task<CampaignMiniResponseDto> UpdatedCampaign(UpdatedCampaignCommand request, CancellationToken cancellationToken);
    Task<CampaignResponseDto> GetCampaignDetail(Guid id, CancellationToken cancellationToken);
    Task<PagedList<CampaignMiniResponseDto>> GetCampaignList(GetCampaignListQuery request, CancellationToken cancellationToken);
    Task<Campaign?> GetCampainByIdAsync(Guid id, CancellationToken cancellationToken);
    Task EnsureCampainExist(Guid id, CancellationToken cancellationToken);
}
