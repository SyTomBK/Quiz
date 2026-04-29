using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Leads;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Contracts.Persistence;

public interface ILeadRepository
{
    Task<Lead?> GetByLeadIdAsync(Guid leadId, CancellationToken cancellationToken);
    Task AddAsync(Lead lead, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<PagedList<LeadMiniResponseDto>> GetLeadList(GetLeadListQuery request, CancellationToken cancellationToken);
}
