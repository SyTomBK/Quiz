namespace QuizSvc.Domain.AuditableEntity;
public class AuditableEntity<T> : Entity<T>
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}
