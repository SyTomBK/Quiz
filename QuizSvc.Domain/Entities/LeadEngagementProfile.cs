using QuizSvc.Domain.AuditableEntity;

namespace QuizSvc.Domain.Entities;

public class LeadEngagementProfile : AuditableEntity<Guid>
{
    public Guid LeadId { get; set; }
    public double OverallEngagementScore { get; set; }
    public DateTime LastCalculatedAt { get; set; }
    public List<InterestAffinity>? InterestAffinities { get; set; }
    public Lead Lead { get; set; } = default!;
}

public class InterestAffinity
{
    public Guid DimensionId { get; set; }
    public double Score { get; set; }
}
