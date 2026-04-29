using QuizSvc.Application.Common.Exceptions;

namespace QuizSvc.Application.Commands.Recommendations;
public record DeleteRecommendationCommand(Guid Id) : IRequest<bool>;

public class DeleteRecommendationHandler : IRequestHandler<DeleteRecommendationCommand, bool>
{
    private readonly IRecommendationRepository _recommendationRepository;
    public DeleteRecommendationHandler(IRecommendationRepository recommendationRepository)
    {
        _recommendationRepository = recommendationRepository;
    }

    public async Task<bool> Handle(DeleteRecommendationCommand request, CancellationToken cancellationToken)
    {
        var count = await _recommendationRepository.DeleteByIdAsync(request.Id, cancellationToken);

        if(count == 0)
            GrpcExceptions.NotFound("Recommendation", request.Id);

        return count > 0;
    }
}
