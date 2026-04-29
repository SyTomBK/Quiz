using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Recommendations;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Contracts.Persistence;

public interface IRecommendationRepository
{
    Task<List<Recommendation>> GetRecommendationsAsync(string tenant, RecommendationType? type, CancellationToken cancellationToken);
    Task<Recommendation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Recommendation recommendation, CancellationToken cancellationToken);
    Task<int> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedList<RecommendationMiniResponseDto>> GetRecommendationList(GetRecommendationListQuery request, CancellationToken cancellationToken);
}