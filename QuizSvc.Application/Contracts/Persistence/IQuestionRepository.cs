namespace QuizSvc.Application.Contracts.Persistence;

public interface IQuestionRepository
{
    Task AddAsync(Question request, CancellationToken cancellationToken);
    Task<int> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Question?> GetWithIncludeByIdAsync(Guid id, CancellationToken cancellationToken);
}
