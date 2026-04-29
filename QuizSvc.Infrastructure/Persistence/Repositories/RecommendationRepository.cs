using Microsoft.EntityFrameworkCore.Query.Internal;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Recommendations;
using QuizSvc.Share.Utils;

namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class RecommendationRepository : IRecommendationRepository
{
    private readonly DataContext _context;

    public RecommendationRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Recommendation>> GetRecommendationsAsync(string tenant, RecommendationType? type, CancellationToken cancellationToken)
    {
        var query = _context.Recommendations
            .Where(x => x.Tenant == tenant && x.IsActive);

        if (type.HasValue && type != RecommendationType.Unknown)
        {
            query = query.Where(x => x.Type == type.Value);
        }

        return await query
            .OrderBy(x => x.Order)
            .ThenByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
    public async Task<Recommendation?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Recommendations.FindAsync([id], cancellationToken);
    }

    public async Task AddAsync(Recommendation recommendation, CancellationToken cancellationToken)
    {
        await _context.Recommendations.AddAsync(recommendation, cancellationToken);
    }

    public async Task<int> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var affectedRows = await _context.Recommendations
            .Where(q => q.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return affectedRows;
    }

    public async Task<PagedList<RecommendationMiniResponseDto>> GetRecommendationList(GetRecommendationListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Recommendations
           .Where(x => x.Tenant == request.Tenant)
              .AsQueryable().AsNoTracking()
              .WhereIfHasValue<Recommendation, RecommendationType>(request.Type, x => x.Type == request.Type)
              .WhereIfNotEmpty(request.Title, x => EF.Functions.ILike(x.Title, $"%{request.Title}%"))
              .WhereIf(request.IsActive != null, x => x.IsActive == request.IsActive)
              .WhereIfNotEmpty(request.DimensionId, x => x.DimensionId == request.DimensionId)
              .WhereIfNotEmpty(request.QuizId, x => x.QuizId == request.QuizId)
              .WhereIfNotEmpty(request.CampaignId, x => x.CampaignId == request.CampaignId)
              .WhereIf(request.FromDate.HasValue, x => x.CreatedAt >= request.FromDate!.Value.Date)
              .WhereIf(request.ToDate.HasValue, x => x.CreatedAt <= request.ToDate!.Value.Date);

        var total = await query.CountAsync(cancellationToken);


        var data = query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(item => new RecommendationMiniResponseDto
        {
            Id = item.Id,
            Tenant = item.Tenant,
            Title = item.Title,
            Type = item.Type,
            Description = item.Description, 
            Order = item.Order,
            CreatedAt = item.CreatedAt,
        });

        return PagedList<RecommendationMiniResponseDto>.Create(data, total, request.Page, request.PageSize);
    }
}
