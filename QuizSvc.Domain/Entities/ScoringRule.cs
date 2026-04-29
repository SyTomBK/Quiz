namespace QuizSvc.Domain.Entities;

public class ScoringRule : AuditableEntity<Guid>
{
    public string Tenant { get; set; } = default!;
    public string RuleKey { get; set; } = default!; // e.g., TIME_SPENT_5S, CLICK_LINK
    public string? Description { get; set; }
    public double Points { get; set; }
    public bool IsActive { get; set; }
}
