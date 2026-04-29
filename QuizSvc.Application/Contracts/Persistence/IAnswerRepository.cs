namespace QuizSvc.Application.Contracts.Persistence;

public interface IAnswerRepository
{
    Task DeleteByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
}
