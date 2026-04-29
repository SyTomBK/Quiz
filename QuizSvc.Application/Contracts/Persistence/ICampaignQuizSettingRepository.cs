namespace QuizSvc.Application.Contracts.Persistence;

public interface ICampaignQuizSettingRepository
{
    Task DeletedCampainQuizSettingIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
    Task<CampaignQuizSetting?> GetCampainQuizSettingByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<CampaignQuizSetting>> GetByIdsAsync(List<Guid>ids, CancellationToken cancellationToken);
    Task<CampaignQuizSetting> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(CampaignQuizSetting request, CancellationToken cancellationToken);
    Task <bool> ExistByCampaignAndQuizAsync(Guid campaignId, Guid quizId, CancellationToken cancellationToken);
    Task<int> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
}
