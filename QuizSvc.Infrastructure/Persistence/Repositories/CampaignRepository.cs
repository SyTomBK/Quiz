using QuizSvc.Application.Commands.Campaigns;
using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Campains;
using QuizSvc.Share.Utils;

namespace QuizSvc.Infrastructure.Persistence.Repositories;
public class CampaignRepository : ICampaignRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public CampaignRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddAsync(Campaign campaign, CancellationToken cancellationToken)
    {
        await _context.Campaigns.AddAsync(campaign, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<CampaignResponseDto> GetCampaignDetail(Guid id, CancellationToken cancellationToken)
    {
        var result = await _context.Campaigns
                .Include(x => x.CampaignQuizSettings)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        if (result == null) throw GrpcExceptions.NotFound("Campaign", id);

        return _mapper.Map<CampaignResponseDto>(result);
    }

    public async Task<CampaignMiniResponseDto> UpdatedCampaign(UpdatedCampaignCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Campaigns
            .Include(x => x.CampaignQuizSettings)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if(entity == null) throw GrpcExceptions.NotFound("Campaign", request.Id);

        var existingQuizIds = string.Join(", ", entity.CampaignQuizSettings.Select(x => x.QuizId.ToString()));
        _mapper.Map(request, entity);


        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CampaignMiniResponseDto>(entity);
    }

    public async Task<PagedList<CampaignMiniResponseDto>> GetCampaignList(GetCampaignListQuery request, CancellationToken cancellationToken)
    {

        var query = _context.Campaigns
            .Where(x => x.Tenant == request.Tenant)
               .AsQueryable().AsNoTracking()
               .WhereIfNotEmpty(request.Code, x => EF.Functions.ILike(x.Code, $"%{request.Code}%"))
               .WhereIf(request.Status.HasValue && request.Status != 0, x => x.Status == request.Status!.Value)
               .WhereIfNotEmpty(request.Name, x => EF.Functions.ILike(x.Name, $"%{request.Name}%"))
               .WhereIf(request.FromDate.HasValue, x => x.CreatedAt >= request.FromDate!.Value.Date)
               .WhereIf(request.ToDate.HasValue, x => x.CreatedAt <= request.ToDate!.Value.Date);

        var total = await query.CountAsync(cancellationToken);

        var data = await query
         .OrderByDescending(x => x.CreatedAt)
         .Skip((request.Page - 1) * request.PageSize)
         .Take(request.PageSize)
         .Select(item => new CampaignMiniResponseDto
         {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Status = item.Status, 
            Address = item.Address,
            StartTime = item.StartTime,
            EndTime = item.EndTime,
            CreatedAt = item.CreatedAt,
         })
         .ToListAsync(cancellationToken);

        return PagedList<CampaignMiniResponseDto>.Create(data, total, request.Page, request.PageSize);
    }

    public async Task<Campaign?> GetCampainByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Campaigns.FindAsync([id], cancellationToken);
    }

    public async Task EnsureCampainExist(Guid id, CancellationToken cancellationToken)
    {
        var exists = await _context.Campaigns.AnyAsync(q => q.Id == id, cancellationToken);

        if (!exists)
            throw GrpcExceptions.NotFound("Campaign", id);
    }
}