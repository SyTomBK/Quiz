namespace QuizSvc.Application.Queries.Recommendations;

public record GetRecommendationDetailQuery(Guid Id) : IRequest<Recommendation?>;

public class GetRecommendationDetailHandler : IRequestHandler<GetRecommendationDetailQuery, Recommendation?>
{
    private readonly IRecommendationRepository _recommendationRepository;

    public GetRecommendationDetailHandler(IRecommendationRepository recommendationRepository)
    {
        _recommendationRepository = recommendationRepository;
    }

    public async Task<Recommendation?> Handle(GetRecommendationDetailQuery request, CancellationToken cancellationToken)
    {
        return await _recommendationRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
