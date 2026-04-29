namespace QuizSvc.Domain.Entities;
public class Recommendation : AuditableEntity<Guid>
{
    public string Tenant { get; set; } = default!;
    public RecommendationType Type { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Image { get; set; }
    public string? CtaText { get; set; }
    public string JsonContent { get; set; } = "{}"; // JSONB column
    public bool IsActive { get; set; } = true;
    public int Order { get; set; }
    public Guid? CampaignId { get; set; }
    public Guid? QuizId { get; set; }
    public Guid? DimensionId { get; set; }
}