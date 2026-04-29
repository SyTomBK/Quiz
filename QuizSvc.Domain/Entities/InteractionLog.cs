namespace QuizSvc.Domain.Entities;

public class InteractionLog : AuditableEntity<Guid>
{
    public Guid LeadId { get; set; } // Required
    public string Tenant { get; set; } = default!; // Required for rule lookup
    public Guid? QuizAttemptId { get; set; } // Optional
    public string EventType { get; set; } = default!; // TIME_SPENT, CLICK, HOVER, etc.
    public Guid? TargetId { get; set; } // QuestionId or AnswerId
    public double Value { get; set; } // e.g., seconds for TIME_SPENT
    public string? Description { get; set; } // Event detail from FE
    public bool IsProcessed { get; set; }
    public QuizAttempt? QuizAttempt { get; set; }
}
