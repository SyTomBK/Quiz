namespace QuizSvc.Application.Queries.Recommendations;

public class GetRecommendationsQuery : IRequest<List<Recommendation>>
{
    public string Tenant { get; set; } = default!;
    public RecommendationType? Type { get; set; }
}

public class GetRecommendationsQueryHandler : IRequestHandler<GetRecommendationsQuery, List<Recommendation>>
{
    private readonly IRecommendationRepository _recommendationRepository;

    public GetRecommendationsQueryHandler(IRecommendationRepository recommendationRepository)
    {
        _recommendationRepository = recommendationRepository;
    }

    public async Task<List<Recommendation>> Handle(GetRecommendationsQuery request, CancellationToken cancellationToken)
    {
        return await _recommendationRepository.GetRecommendationsAsync(request.Tenant, request.Type, cancellationToken);
    }
}
