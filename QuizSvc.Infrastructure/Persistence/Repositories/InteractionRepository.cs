namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class InteractionRepository : IInteractionRepository
{
    private readonly DataContext _context;

    public InteractionRepository(DataContext context)
    {
        _context = context;
    }

    public async Task LogInteraction(InteractionLog log, CancellationToken cancellationToken)
    {
        _context.InteractionLogs.Add(log);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateAsync(Guid leadId, string eventType, int secondWindow, CancellationToken cancellationToken)
    {
        var windowStart = DateTime.UtcNow.AddSeconds(-secondWindow);
        return await _context.InteractionLogs.AnyAsync(x => 
            x.LeadId == leadId && 
            x.EventType == eventType && 
            x.CreatedAt >= windowStart, 
            cancellationToken);
    }

    public async Task<List<InteractionLog>> GetUnprocessedLogs(int batchSize, CancellationToken cancellationToken)
    {
        return await _context.InteractionLogs
            .Include(x => x.QuizAttempt)
                .ThenInclude(xa => xa!.CampaignQuizSetting)
                    .ThenInclude(cqs => cqs.Campaign)
            .Where(x => !x.IsProcessed)
            .OrderBy(x => x.CreatedAt)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateLogsStatus(List<Guid> logIds, bool isProcessed, CancellationToken cancellationToken)
    {
        var logs = await _context.InteractionLogs
            .Where(x => logIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var log in logs)
        {
            log.IsProcessed = isProcessed;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ScoringRule?> GetScoringRule(string tenant, string ruleKey, CancellationToken cancellationToken)
    {
        return await _context.ScoringRules
            .FirstOrDefaultAsync(x => x.Tenant == tenant && x.RuleKey == ruleKey && x.IsActive, cancellationToken);
    }

    public async Task<List<ScoringRule>> GetScoringRules(string tenant, CancellationToken cancellationToken)
    {
        return await _context.ScoringRules
            .Where(x => x.Tenant == tenant && x.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<ScoringRule> UpsertScoringRule(ScoringRule rule, CancellationToken cancellationToken)
    {
        var existing = await _context.ScoringRules
            .FirstOrDefaultAsync(x => x.Tenant == rule.Tenant && x.RuleKey == rule.RuleKey, cancellationToken);

        if (existing != null)
        {
            existing.Points = rule.Points;
            existing.IsActive = rule.IsActive;
            existing.Description = rule.Description;
            rule = existing;
        }
        else
        {
            _context.ScoringRules.Add(rule);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return rule;
    }

    public async Task<LeadEngagementProfile?> GetLeadEngagementProfile(Guid leadId, CancellationToken cancellationToken)
    {
        return await _context.LeadEngagementProfiles
            .FirstOrDefaultAsync(x => x.LeadId == leadId, cancellationToken);
    }

    public async Task UpdateLeadEngagementProfile(LeadEngagementProfile profile, CancellationToken cancellationToken)
    {
        var existing = await _context.LeadEngagementProfiles
            .FirstOrDefaultAsync(x => x.LeadId == profile.LeadId, cancellationToken);

        if (existing == null)
        {
            _context.LeadEngagementProfiles.Add(profile);
        }
        else
        {
            _context.Entry(existing).CurrentValues.SetValues(profile);
            existing.InterestAffinities = profile.InterestAffinities; // For JSON column
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(List<InteractionLog> data, int totalCount)> GetLeadInteractions(Guid leadId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.InteractionLogs
            .Where(x => x.LeadId == leadId);
            
        int totalCount = await query.CountAsync(cancellationToken);
        
        var data = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
            
        return (data, totalCount);
    }
}
