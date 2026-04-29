using System.ComponentModel;

namespace QuizSvc.Domain.Entities;
public class Question : AuditableEntity<Guid>
{
    public string Content { get; set; } = default!;
    [Description("STT")]
    public int Order { get; set; }
    public QuizType Type { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; } = default!;
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
