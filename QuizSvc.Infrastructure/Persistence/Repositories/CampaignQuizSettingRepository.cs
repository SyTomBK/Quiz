using QuizSvc.Application.Common.Exceptions;

namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class CampaignQuizSettingRepository : ICampaignQuizSettingRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public CampaignQuizSettingRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddAsync(CampaignQuizSetting request, CancellationToken cancellationToken)
    {
        await _context.CampaignQuizSettings.AddAsync(request, cancellationToken);
    }

    public async Task DeletedCampainQuizSettingIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        await _context.CampaignQuizSettings
            .Where(x => ids.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task<bool> ExistByCampaignAndQuizAsync(Guid campaignId, Guid quizId, CancellationToken cancellationToken)
    {
        return _context.CampaignQuizSettings.AnyAsync(x => x.CampaignId == campaignId && x.QuizId == quizId, cancellationToken);
    }

    public async Task<CampaignQuizSetting> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var setting = await _context.CampaignQuizSettings.FindAsync([id], cancellationToken);

        if (setting == null)
            throw GrpcExceptions.NotFound("CampaignQuizSetting", id);

        return setting;
    }

    public Task<List<CampaignQuizSetting>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        return _context.CampaignQuizSettings.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
    }

    public async Task<CampaignQuizSetting?> GetCampainQuizSettingByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.CampaignQuizSettings
            .Include(x => x.Campaign)
            .Include(x => x.Quiz)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<int> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var affectedRows = await _context.CampaignQuizSettings
            .Where(q => q.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return affectedRows;
    }
}
