namespace QuizSvc.Application.Contracts.Persistence;

public interface IAnswerDimensionScoresRepository
{
    Task DeleteByAnswerIdAsync(Guid answerId, CancellationToken cancellationToken);
    Task AddRangeAsync(List<AnswerDimensionScore> entities, CancellationToken cancellationToken);
}
