namespace QuizSvc.Domain.Entities;
public class Campaign : AuditableEntity<Guid>
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string Address { get; set; } = default!;
    public string? Image { get; set; }
    public CampaignStatus Status { get; set; } = default!;
    public string Tenant { get; set; } = default!;
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public ICollection<CampaignQuizSetting> CampaignQuizSettings { get; set; } = new List<CampaignQuizSetting>();
}
