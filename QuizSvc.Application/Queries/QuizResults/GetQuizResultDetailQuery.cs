using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.QuizResults;

public class GetQuizResultDetailQuery : IRequest <QuizResultResponseDto>
{
    public Guid Id { get; set; }
}

public class GetQuizResultDetailQueryHandler : IRequestHandler<GetQuizResultDetailQuery, QuizResultResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizResultRepository _quizResultRepository;
    public GetQuizResultDetailQueryHandler(IMapper mapper, IQuizResultRepository quizResultRepository)
    {
        _mapper = mapper;
        _quizResultRepository = quizResultRepository;
    }

    public async Task<QuizResultResponseDto> Handle(GetQuizResultDetailQuery request, CancellationToken cancellationToken)
    {
        var result = await _quizResultRepository.GetQuizResultDetail(request.Id, cancellationToken);
        return result;
    }
}
