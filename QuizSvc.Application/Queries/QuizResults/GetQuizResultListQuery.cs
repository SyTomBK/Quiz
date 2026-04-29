using QuizSvc.Application.Dtos;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Queries.QuizResults;

public class GetQuizResultListQuery : IRequest<PagedList<CampaignQuizResultResponseDto>>
{
    public Guid? LeadId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? QuizId { get; set; }
    public Guid? CampaignId { get; set; }
    public required string Tenant { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}

public class GetQuizResultListQueryHandler : IRequestHandler<GetQuizResultListQuery, PagedList<CampaignQuizResultResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IQuizResultRepository _quizResultRepository;
    public GetQuizResultListQueryHandler(IMapper mapper, IQuizResultRepository quizResultRepository)
    {
        _mapper = mapper;
        _quizResultRepository = quizResultRepository;
    }

    public async Task<PagedList<CampaignQuizResultResponseDto>> Handle(GetQuizResultListQuery request, CancellationToken cancellationToken)
    {
        var result = await _quizResultRepository.GetQuizResultList(request, cancellationToken);
        return result;
    }
}
