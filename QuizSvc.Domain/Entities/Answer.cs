namespace QuizSvc.Domain.Entities;
public class Answer : AuditableEntity<Guid>
{
    public string Content { get; set; } = default!;
    public bool IsCorrect { get; set; } = false;
    public decimal Score { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = default!;
    public ICollection<AnswerDimensionScore> AnswerDimensionScores { get; set; } = new List<AnswerDimensionScore>();
}
