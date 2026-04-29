using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.QuizResults;
using QuizSvc.Share.Utils;

namespace QuizSvc.Infrastructure.Persistence.Repositories;
public class QuizResultRepository : IQuizResultRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public QuizResultRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<QuizResultResponseDto> GetQuizResultDetail(Guid id, CancellationToken cancellationToken)
    {
        var result = await _context.QuizResults
            .Include(x => x.QuizAttempt)
                .ThenInclude(x => x!.Lead)
            .Include(x => x.QuizAttempt)
                .ThenInclude(x => x!.User)
            .Include(x => x.QuizAttempt)
                .ThenInclude(x => x!.CampaignQuizSetting)
                  .ThenInclude(x => x.Quiz)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
     
        if (result == null) 
            throw GrpcExceptions.NotFound("Quiz Result", id);

        var response = _mapper.Map<QuizResultResponseDto>(result);
        
        if (result.QuizAttempt != null)
        {
            response.LeadId = result.QuizAttempt.Lead?.LeadId;
            response.UserId = result.QuizAttempt.User?.UserId;
            response.QuizId = result.QuizAttempt.CampaignQuizSetting.QuizId;
            response.QuizTitle = result.QuizAttempt.CampaignQuizSetting?.Quiz?.Title ?? string.Empty;
        }

        var scoreLookup = result.DimensionScoreResults!
            .ToDictionary(x => x.DimensionId, x => x.Score);

        var dimensions = await _context.Dimensions
            .Where(x => scoreLookup.Keys.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var dimensionResults = dimensions
            .Select(d => new DimensionScoreResultDto
            {
                DimensionId = d.Id,
                DimensionTitle = d.Title,
                Score = scoreLookup[d.Id]
            })
            .ToList();

        response.DimensionScoreResults = dimensionResults;

        return response;
    }

    public async Task<PagedList<CampaignQuizResultResponseDto>> GetQuizResultList2(GetQuizResultListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.QuizResults
            .Where(x => x.QuizAttempt!.CampaignQuizSetting.Campaign.Tenant == request.Tenant)
               .AsQueryable().AsNoTracking()
               .WhereIfNotEmpty(request.LeadId, x => x.QuizAttempt!.LeadId == request.LeadId)
               .WhereIfNotEmpty(request.UserId, x => x.QuizAttempt!.UserId == request.UserId)
               .WhereIfNotEmpty(request.QuizId, x => x.QuizAttempt!.CampaignQuizSetting.QuizId == request.QuizId)
               .WhereIfNotEmpty(request.CampaignId, x => x.QuizAttempt!.CampaignQuizSetting.CampaignId == request.CampaignId)
               .WhereIf(request.FromDate.HasValue, x => x.CreatedAt >= request.FromDate!.Value.Date)
               .WhereIf(request.ToDate.HasValue, x => x.CreatedAt <= request.ToDate!.Value.Date);

        var total = await query.CountAsync(cancellationToken);

        var entities = await query
           .Include(x => x.QuizAttempt)!
               .ThenInclude(x => x!.CampaignQuizSetting)!
                   .ThenInclude(x => x.Campaign)
           .Include(x => x.QuizAttempt)!
               .ThenInclude(x => x!.CampaignQuizSetting)!
                   .ThenInclude(x => x.Quiz)
           .Include(x => x.DimensionScoreResults)
           .OrderByDescending(x => x.CreatedAt)
           .Skip((request.Page - 1) * request.PageSize)
           .Take(request.PageSize)
           .ToListAsync(cancellationToken);

        var data = entities.Select(item => new CampaignQuizResultResponseDto
        {
            Id = item.Id,
            QuizAttemptId = item.QuizAttemptId,
            CampaignName = item.QuizAttempt!.CampaignQuizSetting.Campaign.Name,
            QuizTitle = item.QuizAttempt.CampaignQuizSetting.Quiz.Title,
            TotalScore = item.TotalScore,
            Tenant = item.QuizAttempt.CampaignQuizSetting.Campaign.Tenant,
            DimensionScoreResults = item.DimensionScoreResults!
            .Select(d => new DimensionScoreResultDto
            {
                DimensionId = d.DimensionId,
                Score = d.Score
            })
            .ToList() ?? []
        }).ToList();

        return PagedList<CampaignQuizResultResponseDto>.Create(data, total, request.Page, request.PageSize);
    }

    public async Task<PagedList<CampaignQuizResultResponseDto>> GetQuizResultList(GetQuizResultListQuery request, CancellationToken cancellationToken)
    {
        var totalAllRaw = await _context.QuizResults.CountAsync(cancellationToken);
        Console.WriteLine($"[Repo-Debug] Total QuizResults in DB (No Filter): {totalAllRaw}");
        Console.WriteLine($"[Repo-Debug] Search Filter -> Tenant: '{request.Tenant}', LeadId: '{request.LeadId}', QuizId: '{request.QuizId}'");
        
        var baseQuery = _context.QuizResults
            .AsNoTracking()
            .Where(x => x.QuizAttempt!.CampaignQuizSetting.Campaign.Tenant == request.Tenant)
            .WhereIfNotEmpty(request.LeadId, x => x.QuizAttempt!.Lead!.LeadId == request.LeadId)
            .WhereIfNotEmpty(request.UserId, x => x.QuizAttempt!.User!.UserId == request.UserId)
            .WhereIfNotEmpty(request.QuizId, x => x.QuizAttempt!.CampaignQuizSetting.QuizId == request.QuizId)
            .WhereIfNotEmpty(request.CampaignId, x => x.QuizAttempt!.CampaignQuizSetting.CampaignId == request.CampaignId)
            .WhereIf(request.FromDate.HasValue, x => x.CreatedAt >= request.FromDate!.Value.Date)
            .WhereIf(request.ToDate.HasValue, x => x.CreatedAt <= request.ToDate!.Value.Date);

        // 1. COUNT (nhẹ)
        var total = await baseQuery.CountAsync(cancellationToken);

        // 2. PAGING lấy ID trước (tối ưu quan trọng)
        var ids = await baseQuery
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        // 3. LOAD FULL DATA theo ID
        var quizResults = await _context.QuizResults
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .Include(x => x.QuizAttempt)
                .ThenInclude(x => x!.CampaignQuizSetting)
                    .ThenInclude(x => x.Campaign)
            .Include(x => x.QuizAttempt)
                .ThenInclude(x => x!.CampaignQuizSetting)
                    .ThenInclude(x => x.Quiz)
            .Include(x => x.QuizAttempt)
                .ThenInclude(x => x!.Lead)
            .Include(x => x.QuizAttempt)
                .ThenInclude(x => x!.User)
            .ToListAsync(cancellationToken);

        // 4. Dimension lookup 
        var dimensionIds = quizResults
            .Where(x => x.DimensionScoreResults != null)
            .SelectMany(x => x.DimensionScoreResults!)
            .Select(x => x.DimensionId)
            .Distinct()
            .ToList();

        var dimensionLookup = await _context.Dimensions
            .AsNoTracking()
            .Where(x => dimensionIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.Title, cancellationToken);

        // 5. MAP DTO
        var data = quizResults.Select(item => new CampaignQuizResultResponseDto
        {
            Id = item.Id,
            LeadName = item.QuizAttempt?.Lead?.FullName,
            Username = item.QuizAttempt?.User?.Username,
            QuizAttemptId = item.QuizAttemptId,
            CampaignName = item.QuizAttempt!.CampaignQuizSetting.Campaign.Name,
            QuizTitle = item.QuizAttempt.CampaignQuizSetting.Quiz.Title,
            TotalScore = item.TotalScore,
            Tenant = item.QuizAttempt.CampaignQuizSetting.Campaign.Tenant,
            CreatedAt = item.CreatedAt,
            DimensionScoreResults = item.DimensionScoreResults?
                .Select(d => new DimensionScoreResultDto
                {
                    DimensionId = d.DimensionId,
                    DimensionTitle = dimensionLookup.TryGetValue(d.DimensionId, out var title)
                        ? title: string.Empty,
                    Score = d.Score
                })
                .ToList()
        }).ToList();

        // 6. RETURN PAGED
        return PagedList<CampaignQuizResultResponseDto>.Create( data, total, request.Page, request.PageSize);
    }
}
