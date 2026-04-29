using QuizSvc.Application.Dtos;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Queries.Quizs;
public class GetQuizListQuery : IRequest<PagedList<QuizMiniResponseDto>>
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Tenant { get; set; }
    public QuizType? Type { get; set; }
    public List<QuizSource>? Sources { get; set; }
    public Guid? CampaignId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}

public class GetQuizListQueryHandler : IRequestHandler<GetQuizListQuery, PagedList<QuizMiniResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IQuizRepository _quizRepository;
    public GetQuizListQueryHandler(IMapper mapper, IQuizRepository quizRepository)
    {
        _mapper = mapper;
        _quizRepository = quizRepository;
    }
    public async Task<PagedList<QuizMiniResponseDto>> Handle(GetQuizListQuery request, CancellationToken cancellationToken)
    {
        var result = await _quizRepository.GetQuizList(request, cancellationToken);
        return result;
    }
}
