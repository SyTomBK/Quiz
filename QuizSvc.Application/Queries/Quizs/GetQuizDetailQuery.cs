using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.Quizs;

public class GetQuizDetailQuery : IRequest<QuizResponseDto>
{
    public required Guid Id { get; set; }
}
public class GetQuizDetailQueryHandler : IRequestHandler<GetQuizDetailQuery, QuizResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizRepository _quizRepository;
    public GetQuizDetailQueryHandler(IMapper mapper, IQuizRepository quizRepository)
    {
        _mapper = mapper;
        _quizRepository = quizRepository;
    }
    public async Task<QuizResponseDto> Handle(GetQuizDetailQuery request, CancellationToken cancellationToken)
    {
        var result = await _quizRepository.GetQuizDetail(request.Id, cancellationToken);
        return result;
    }
}
