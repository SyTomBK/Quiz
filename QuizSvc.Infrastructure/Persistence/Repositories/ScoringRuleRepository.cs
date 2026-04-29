using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.ScoringRules;
using QuizSvc.Share.Utils;

namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class ScoringRuleRepository : IScoringRuleRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public ScoringRuleRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task AddAsync(ScoringRule request, CancellationToken cancellationToken)
    {
        await _context.ScoringRules.AddAsync(request, cancellationToken);
    }

    public async Task<int> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var affectedRows = await _context.ScoringRules
            .Where(q => q.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return affectedRows;
    }

    public async Task<bool> ExistByRuleKeyAndTenant(string ruleKey, string tenant, CancellationToken cancellationToken)
    {
        return await _context.ScoringRules.AnyAsync(x => x.RuleKey == ruleKey && x.Tenant == tenant, cancellationToken);
    }

    public async Task<ScoringRule?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.ScoringRules.FindAsync([id], cancellationToken);
    }

    public async Task<PagedList<ScoringRuleMiniResponseDto>> GetScoringRuleList(GetScoringRuleListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ScoringRules
            .Where(x => x.Tenant == request.Tenant)
            .AsQueryable().AsNoTracking()
            .WhereIfNotEmpty(request.RuleKey, x => EF.Functions.ILike(x.RuleKey, $"%{request.RuleKey}%"))
            .WhereIf(request.IsActive != null, x => x.IsActive == request.IsActive)
            .WhereIf(request.FromDate.HasValue, x => x.CreatedAt >= request.FromDate!.Value.Date)
            .WhereIf(request.ToDate.HasValue, x => x.CreatedAt <= request.ToDate!.Value.Date);

        var total = await query.CountAsync(cancellationToken);

        var data = query
          .OrderByDescending(x => x.CreatedAt)
          .Skip((request.Page - 1) * request.PageSize)
          .Take(request.PageSize)
          .Select(item => new ScoringRuleMiniResponseDto
        {
            Id = item.Id,
            Tenant = item.Tenant,
            RuleKey = item.RuleKey,
            Description = item.Description,
            Points = item.Points,
            IsActive = item.IsActive,
            CreatedAt = item.CreatedAt,
        });

        return PagedList<ScoringRuleMiniResponseDto>.Create(data, total, request.Page, request.PageSize);
    }
}