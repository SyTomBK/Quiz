namespace QuizSvc.Domain.Entities;

public class QuizResult : AuditableEntity<Guid>
{
    public Guid QuizAttemptId { get; set; }
    public decimal TotalScore { get; set; }
    public List<DimensionScoreResult>? DimensionScoreResults { get; set; }
    public QuizAttempt? QuizAttempt { get; set; }
}

public class DimensionScoreResult
{
    public Guid DimensionId { get; set; }
    public decimal Score { get; set; }
}
