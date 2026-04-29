namespace QuizSvc.Domain.Entities;
public class AnswerDimensionScore : Entity<Guid>
{
    public decimal Score { get; set; }
    public Guid DimensionId { get; set; }
    public Guid AnswerId { get; set; }
    public Answer Answer { get; set; } = default!;
    public Dimension Dimension { get; set; } = default!;
}
