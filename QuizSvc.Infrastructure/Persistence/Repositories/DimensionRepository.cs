using QuizSvc.Application.Commands.Dimensions;
using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Dimensions;
using QuizSvc.Share.Utils;

namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class DimensionRepository : IDimensionRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public DimensionRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<DimensionMiniResponseDto> CreateDimention(CreatedDimensionCommand request, CancellationToken cancellationToken)
    {
        var dimension = _mapper.Map<Dimension>(request);
        _context.Dimensions.Add(dimension);
        await _context.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<DimensionMiniResponseDto>(dimension);
    }

    public async Task<List<Dimension>> GetByQuizId(Guid quizId, CancellationToken cancellationToken)
    {
        return await _context.Dimensions
            .Where(x => x.QuizId == quizId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddRangeAsync(List<Dimension> dimensions, CancellationToken cancellationToken)
    {
        await _context.Dimensions.AddRangeAsync(dimensions, cancellationToken);
    }

    public void RemoveRange(List<Dimension> dimensions)
    {
        _context.Dimensions.RemoveRange(dimensions);
    }

    public async Task<List<Guid>> GetDimensionIdsByQuizId(Guid quizId, CancellationToken cancellationToken)
    {
        return await _context.Dimensions
            .Where(d => d.QuizId == quizId)
            .Select(d => d.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dimension?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Dimensions.FindAsync([id], cancellationToken);
    }

    public async Task EnsureDimensionExist(Guid id, CancellationToken cancellationToken)
    {
        var exists = await _context.Dimensions.AnyAsync(x => x.Id == id, cancellationToken);

        if (!exists)
            throw GrpcExceptions.NotFound("Dimension", id);
    }

    public async Task<PagedList<QuizDimensionMiniResponseDto>> GetQuizDimensionList(GetQuizDimensionListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Dimensions
          .Where(x => x.Quiz.Tenant == request.Tenant)
            .WhereIfNotEmpty(request.Title, x => EF.Functions.ILike(x.Title, $"%{request.Title}%"))
            .AsQueryable().AsNoTracking()
            .WhereIf(request.FromDate.HasValue, x => x.CreatedAt >= request.FromDate!.Value.Date)
            .WhereIf(request.ToDate.HasValue, x => x.CreatedAt <= request.ToDate!.Value.Date);

        var total = await query.CountAsync(cancellationToken);


        var data = query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(item => new QuizDimensionMiniResponseDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                QuizId = item.QuizId,
                QuizTitle = item.Quiz.Title
            });

        return PagedList<QuizDimensionMiniResponseDto>.Create(data, total, request.Page, request.PageSize);
    }
}
