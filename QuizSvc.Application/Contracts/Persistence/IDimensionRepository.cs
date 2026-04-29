using QuizSvc.Application.Commands.Dimensions;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Dimensions;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Contracts.Persistence;

public interface IDimensionRepository
{
    Task<DimensionMiniResponseDto> CreateDimention(CreatedDimensionCommand request, CancellationToken cancellationToken);
    Task<List<Dimension>> GetByQuizId(Guid quizId, CancellationToken cancellationToken);
    Task<PagedList<QuizDimensionMiniResponseDto>> GetQuizDimensionList(GetQuizDimensionListQuery request, CancellationToken cancellationToken);
    Task AddRangeAsync(List<Dimension> dimensions, CancellationToken cancellationToken);
    void RemoveRange(List<Dimension> dimensions);
    Task<List<Guid>> GetDimensionIdsByQuizId(Guid quizId, CancellationToken cancellationToken);
    Task<Dimension?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task EnsureDimensionExist(Guid id, CancellationToken cancellationToken);
}
