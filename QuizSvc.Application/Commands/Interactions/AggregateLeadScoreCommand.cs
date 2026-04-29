using MediatR;
using Microsoft.Extensions.Logging;
using QuizSvc.Application.Contracts.Persistence;
using QuizSvc.Domain.Entities;

namespace QuizSvc.Application.Commands.Interactions;

public class AggregateLeadScoreCommand : IRequest<Unit>
{
    public int BatchSize { get; set; } = 100;
}

public class AggregateLeadScoreCommandHandler : IRequestHandler<AggregateLeadScoreCommand, Unit>
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly ILeadRepository _leadRepository;
    private readonly ILogger<AggregateLeadScoreCommandHandler> _logger;

    public AggregateLeadScoreCommandHandler(
        IInteractionRepository interactionRepository, 
        IQuizRepository quizRepository,
        ILeadRepository leadRepository,
        ILogger<AggregateLeadScoreCommandHandler> logger)
    {
        _interactionRepository = interactionRepository;
        _quizRepository = quizRepository;
        _leadRepository = leadRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(AggregateLeadScoreCommand request, CancellationToken cancellationToken)
    {
        // 1. Lấy danh sách log chưa xử lý
        var logs = await _interactionRepository.GetUnprocessedLogs(request.BatchSize, cancellationToken);
        if (logs == null || logs.Count == 0) return Unit.Value;

        // Group theo LeadId
        var logsByLead = logs.GroupBy(l => l.LeadId);
        var logsToMarkProcessed = new List<Guid>();

        foreach (var leadGroup in logsByLead)
        {
            var leadIdFromLog = leadGroup.Key; // Đây là LeadId (External GUID) từ log
            var tenantId = leadGroup.First().Tenant;
            
            // 1. KIỂM TRA LEAD TỒN TẠI VÀ LẤY ID NỘI BỘ
            var lead = await _leadRepository.GetByLeadIdAsync(leadIdFromLog, cancellationToken);
            if (lead == null)
            {
                _logger.LogWarning("Lead {LeadId} not found in database. Marking {Count} logs as processed (ignored).", leadIdFromLog, leadGroup.Count());
                logsToMarkProcessed.AddRange(leadGroup.Select(l => l.Id));
                continue;
            }

            // 2. Sử dụng lead.Id (Internal GUID) cho Profile
            try 
            {
                var rules = await _interactionRepository.GetScoringRules(tenantId, cancellationToken);
                
                var profile = await _interactionRepository.GetLeadEngagementProfile(lead.Id, cancellationToken);
                if (profile == null)
                {
                    profile = new LeadEngagementProfile
                    {
                        LeadId = lead.Id, // <-- Sử dụng ID nội bộ
                        OverallEngagementScore = 0,
                        InterestAffinities = new List<InterestAffinity>()
                    };
                }

                foreach (var log in leadGroup)
                {
                    var rule = rules.FirstOrDefault(r => string.Equals(r.RuleKey, log.EventType, StringComparison.OrdinalIgnoreCase) && r.IsActive);
                    if (rule != null)
                    {
                        double pointsToAdd = (log.Value > 0 ? log.Value : 1) * rule.Points;
                        profile.OverallEngagementScore += pointsToAdd;
                        
                        _logger.LogInformation("Lead {LeadId}: Processed {EventType}, added {Points} pts. Total: {Total}", 
                            leadIdFromLog, log.EventType, pointsToAdd, profile.OverallEngagementScore);
                    }
                }

                profile.LastCalculatedAt = DateTime.UtcNow;
                
                await _interactionRepository.UpdateLeadEngagementProfile(profile, cancellationToken);
                logsToMarkProcessed.AddRange(leadGroup.Select(l => l.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aggregating score for Lead {LeadId}", leadIdFromLog);
            }
        }

        // Đánh dấu log đã xử lý hàng loạt
        if (logsToMarkProcessed.Any())
        {
            await _interactionRepository.UpdateLogsStatus(logsToMarkProcessed, true, cancellationToken);
        }

        return Unit.Value;
    }
}
