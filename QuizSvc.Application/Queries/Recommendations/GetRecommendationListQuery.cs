using QuizSvc.Application.Dtos;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Queries.Recommendations;

public class GetRecommendationListQuery : IRequest<PagedList<RecommendationMiniResponseDto>>
{
    public string? Title { get; set; }
    public RecommendationType Type { get; set; }
    public required string Tenant { get; set; }
    public bool? IsActive { get; set; }
    public Guid? CampaignId { get; set; }
    public Guid? QuizId { get; set; }
    public Guid? DimensionId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}

public class GetRecommendationListQueryHandler : IRequestHandler<GetRecommendationListQuery, PagedList<RecommendationMiniResponseDto>>
{
    private readonly IRecommendationRepository _recommendationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecommendationListQueryHandler(
        IRecommendationRepository recommendationRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _recommendationRepository = recommendationRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<PagedList<RecommendationMiniResponseDto>> Handle(GetRecommendationListQuery request, CancellationToken cancellationToken)
    {
        var result = await _recommendationRepository.GetRecommendationList(request, cancellationToken);
        return result;
    }
}
