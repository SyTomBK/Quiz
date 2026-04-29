
namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class AnswerRepository : IAnswerRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public AnswerRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task DeleteByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        await _context.Answers
        .Where(x => ids.Contains(x.Id))
        .ExecuteDeleteAsync(cancellationToken);
    }
}
