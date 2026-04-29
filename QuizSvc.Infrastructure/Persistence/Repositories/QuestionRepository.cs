namespace QuizSvc.Infrastructure.Persistence.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public QuestionRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddAsync(Question request, CancellationToken cancellationToken)
    {
        await _context.Questions.AddAsync(request, cancellationToken);
    }

    public async Task<Question?> GetWithIncludeByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Questions
               .Include(q => q.Answers)
                   .ThenInclude(a => a.AnswerDimensionScores)
               .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task<int> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var affectedRows = await _context.Questions
            .Where(q => q.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return affectedRows;
    }
   
}