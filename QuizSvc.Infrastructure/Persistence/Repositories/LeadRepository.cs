using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Leads;
using QuizSvc.Share.Utils;

namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class LeadRepository : ILeadRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public LeadRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddAsync(Lead lead, CancellationToken cancellationToken)
    {
        await _context.Leads.AddAsync(lead, cancellationToken);
    }

    public async Task<Lead?> GetByLeadIdAsync(Guid leadId, CancellationToken cancellationToken)
    {
        var lead = await _context.Leads.FirstOrDefaultAsync(x => x.LeadId == leadId, cancellationToken);
        return lead;
    }

    public async Task<PagedList<LeadMiniResponseDto>> GetLeadList(GetLeadListQuery request, CancellationToken cancellationToken)
    {
        var leadQuery = _context.Leads.AsQueryable();

        // 1. Filter by Basic Info
        leadQuery = leadQuery
               .WhereIfNotEmpty(request.Key, x => EF.Functions.ILike(x.FullName!, $"%{request.Key}%")
               || x.PhoneNumber!.Contains(request.Key!))
               .WhereIf(request.FromDate.HasValue && !request.IsTopLeads, x => x.CreatedAt >= request.FromDate!.Value.Date)
               .WhereIf(request.ToDate.HasValue && !request.IsTopLeads, x => x.CreatedAt <= request.ToDate!.Value.Date)
               .WhereIfNotEmpty(request.ReferralCode, x => x.ReferralCode == request.ReferralCode)
               .WhereIfNotEmpty(request.Tenant, x => x.Tenant == request.Tenant)
               .WhereIf(request.CustomerId.HasValue, x => x.CustomerId == request.CustomerId);

        // 2. Specialized Logic for Top Leads (Aggregated Score in Period)
        if (request.IsTopLeads)
        {
            var interactionQuery = _context.InteractionLogs
                .WhereIfNotEmpty(request.Tenant, x => x.Tenant == request.Tenant)
                .WhereIf(request.FromDate.HasValue, x => x.CreatedAt >= request.FromDate!.Value)
                .WhereIf(request.ToDate.HasValue, x => x.CreatedAt <= request.ToDate!.Value);

            var scoringRules = await _context.ScoringRules
                .Where(r => r.IsActive)
                .ToListAsync(cancellationToken);

            var ruleLookup = scoringRules
                .ToDictionary(r => $"{r.Tenant}::{r.RuleKey}", r => r.Points);

            var allLogs = await interactionQuery.ToListAsync(cancellationToken);

            // Tính điểm có trọng số (in-memory)
            var scoredLeads = allLogs
                .GroupBy(x => x.LeadId)
                .Select(g => new
                {
                    LeadId = g.Key,
                    TotalScore = g.Sum(x =>
                    {
                        var ruleKey = $"{x.Tenant}::{x.EventType}";
                        double pointWeight = ruleLookup.TryGetValue(ruleKey, out var pts) ? pts : 1.0;
                        return (x.Value > 0 ? x.Value : 1) * pointWeight;
                    })
                })
                .OrderByDescending(x => x.TotalScore)
                .ToList();

            var total = scoredLeads.Count;

            var pagedScores = scoredLeads
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var leadIds = pagedScores.Select(x => x.LeadId).ToList();
            var leadsFromDb = await _context.Leads
                .Where(l => leadIds.Contains(l.LeadId))
                .WhereIfNotEmpty(request.Tenant, l => l.Tenant == request.Tenant)
                .ToListAsync(cancellationToken);

            var data = pagedScores
                .Join(leadsFromDb,
                    s => s.LeadId,
                    l => l.LeadId,
                    (s, l) => new LeadMiniResponseDto
                    {
                        Id = l.Id,
                        LeadId = l.LeadId,
                        IsActive = l.IsActive,
                        LinkedUserId = l.LinkedUserId,
                        FullName = l.FullName,
                        PhoneNumber = l.PhoneNumber,
                        Email = l.Email,
                        SchoolName = l.SchoolName,
                        Avatar = l.Avatar,
                        ReferralCode = l.ReferralCode,
                        Tenant = l.Tenant,
                        CustomerId = l.CustomerId,
                        Status = l.Status,
                        Note = l.Note,
                        TotalScore = s.TotalScore,
                        CreatedAt = l.CreatedAt,
                    })
                .ToList();

            return PagedList<LeadMiniResponseDto>.Create(data, total, request.Page, request.PageSize);
        }

        // 3. Normal List - Join LeadEngagementProfiles để lấy OverallEngagementScore (không tính lại)
        var queryWithScores = from lead in leadQuery
                              join engagement in _context.LeadEngagementProfiles on lead.Id equals engagement.LeadId into engagementGroup
                              from engagement in engagementGroup.DefaultIfEmpty()
                              select new { lead, Score = engagement != null ? engagement.OverallEngagementScore : 0 };

        if (request.SortByScore)
        {
            queryWithScores = queryWithScores.OrderByDescending(x => x.Score);
        }
        else
        {
            queryWithScores = queryWithScores.OrderByDescending(x => x.lead.CreatedAt);
        }

        var countTotal = await queryWithScores.CountAsync(cancellationToken);

        var resultData = await queryWithScores
         .Skip((request.Page - 1) * request.PageSize)
         .Take(request.PageSize)
         .Select(item => new LeadMiniResponseDto
         {
             Id = item.lead.Id,
             LeadId = item.lead.LeadId,
             IsActive = item.lead.IsActive,
             LinkedUserId = item.lead.LinkedUserId,
             FullName = item.lead.FullName,
             PhoneNumber = item.lead.PhoneNumber,
             Email = item.lead.Email,
             SchoolName = item.lead.SchoolName,
             Avatar = item.lead.Avatar,
             ReferralCode = item.lead.ReferralCode,
             Tenant = item.lead.Tenant,
             CustomerId = item.lead.CustomerId,
             Status = item.lead.Status,
             Note = item.lead.Note,
             TotalScore = item.Score,
             CreatedAt = item.lead.CreatedAt,
         })
         .ToListAsync(cancellationToken);

        return PagedList<LeadMiniResponseDto>.Create(resultData, countTotal, request.Page, request.PageSize);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
