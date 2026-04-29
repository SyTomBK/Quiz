
namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class AnswerDimensionScoresRepository : IAnswerDimensionScoresRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public AnswerDimensionScoresRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddRangeAsync(List<AnswerDimensionScore> entities, CancellationToken cancellationToken)
    {
        await _context.AnswerDimensionScores.AddRangeAsync(entities, cancellationToken);
    }

    public async Task DeleteByAnswerIdAsync(Guid answerId, CancellationToken cancellationToken)
    {
        await _context.AnswerDimensionScores
        .Where(x => x.AnswerId == answerId)
        .ExecuteDeleteAsync(cancellationToken);
    }
}
