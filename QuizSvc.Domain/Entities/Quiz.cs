    namespace QuizSvc.Domain.Entities;
public class Quiz : AuditableEntity<Guid>
{
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? Instruction { get; set; }
    public string? Image { get; set; }
    public QuizType Type { get; set; }
    public string? Tenant { get; set; }
    public Guid? ParentId { get; set; }
    public QuizSource Source { get; set; }
    public decimal EstimateTime { get; set; }
    public ICollection<Dimension>? Dimensions { get; set; } = new List<Dimension>();
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<CampaignQuizSetting> CampaignQuizSettings { get; set; } = new List<CampaignQuizSetting>();
}