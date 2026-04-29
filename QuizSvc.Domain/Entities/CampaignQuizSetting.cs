namespace QuizSvc.Domain.Entities;

public class CampaignQuizSetting : Entity<Guid>
{
    public Guid CampaignId { get; set; }
    public Guid QuizId { get; set; }
    public bool IsActive { get; set; } = true;
    public int MaxAttempts { get; set; }
    public LeadCollectionPolicy? LeadCollectionPolicy { get; set; }
    public CheckpointConfig? CheckpointConfig { get; set; }
    public virtual Campaign Campaign { get; set; } = null!;
    public virtual Quiz Quiz { get; set; } = null!;
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
