namespace QuizSvc.Domain.Entities;
public class Dimension : AuditableEntity<Guid>
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public Guid QuizId { get; set; }
    public Quiz Quiz { get; set; } = default!;
    public ICollection<AnswerDimensionScore> AnswerDimensionScores { get; set; } = new List<AnswerDimensionScore>();
}
