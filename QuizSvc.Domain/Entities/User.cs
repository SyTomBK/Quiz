using System.ComponentModel;

namespace QuizSvc.Domain.Entities;
public class User : AuditableEntity<Guid>
{
    [Description("UserId lấy từ SSO")]
    public Guid UserId { get; set; }
    public string Username { get; set; } = default!;
    public string Tenant { get; set; } = default!;
    public bool IsActive { get; set; }
    public Lead? Lead { get; set; }
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
