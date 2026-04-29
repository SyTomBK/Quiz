using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.ScoringRules;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Contracts.Persistence;
public interface IScoringRuleRepository
{
    Task AddAsync(ScoringRule request, CancellationToken cancellationToken);
    Task<ScoringRule?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<int> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedList<ScoringRuleMiniResponseDto>> GetScoringRuleList(GetScoringRuleListQuery request, CancellationToken cancellationToken);
    Task<bool> ExistByRuleKeyAndTenant(string ruleKey, string tenant, CancellationToken cancellationToken);
}
