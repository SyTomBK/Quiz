namespace QuizSvc.Domain.Entities;

public class QuizAttempt : AuditableEntity<Guid>
{
    public Guid CampaignQuizSettingId { get; set; }
    public Guid? LeadId { get; set; }
    public Guid? UserId { get; set; }
    public AttemptStatus AttemptStatus { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? CurrentQuestionId { get; set; }
    public List<UserAnswer>? UserAnswers { get; set; }
    public CampaignQuizSetting CampaignQuizSetting { get; set; } = default!;
    public Lead? Lead { get; set; }
    public User? User { get; set; }
    public Question? CurrentQuestion { get; set; }
    public QuizResult? QuizResult { get; set; }
}

public class UserAnswer
{
    public Guid QuestionId { get; set; }
    public Guid AnswerId { get; set; }
}
