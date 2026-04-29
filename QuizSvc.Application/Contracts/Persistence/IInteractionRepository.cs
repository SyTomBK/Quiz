using QuizSvc.Domain.Entities;

namespace QuizSvc.Application.Contracts.Persistence;

public interface IInteractionRepository
{
    Task LogInteraction(InteractionLog log, CancellationToken cancellationToken);
    Task<bool> IsDuplicateAsync(Guid leadId, string eventType, int secondWindow, CancellationToken cancellationToken);
    Task<List<InteractionLog>> GetUnprocessedLogs(int batchSize, CancellationToken cancellationToken);
    Task UpdateLogsStatus(List<Guid> logIds, bool isProcessed, CancellationToken cancellationToken);
    
    Task<ScoringRule?> GetScoringRule(string tenant, string ruleKey, CancellationToken cancellationToken);
    Task<List<ScoringRule>> GetScoringRules(string tenant, CancellationToken cancellationToken);
    Task<ScoringRule> UpsertScoringRule(ScoringRule rule, CancellationToken cancellationToken);
    
    Task<LeadEngagementProfile?> GetLeadEngagementProfile(Guid leadId, CancellationToken cancellationToken);
    Task UpdateLeadEngagementProfile(LeadEngagementProfile profile, CancellationToken cancellationToken);
    Task<(List<InteractionLog> data, int totalCount)> GetLeadInteractions(Guid leadId, int page, int pageSize, CancellationToken cancellationToken);
}
