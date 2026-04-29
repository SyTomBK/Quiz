using Microsoft.Extensions.Logging;
using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;
using System.Text.Json;

namespace QuizSvc.Application.Commands.Recommendations;

public class UpdateRecommendationCommand : IRequest<RecommendationMiniResponseDto>
{
    public Guid Id { get; set; }
    public string Tenant { get; set; } = default!;
    public RecommendationTypeProto Type { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Image { get; set; }
    public string? CtaText { get; set; }
    public string JsonContent { get; set; } = "{}";
    public int Order { get; set; }
    public Guid? CampaignId { get; set; }
    public Guid? QuizId { get; set; }
    public Guid? DimensionId { get; set; }
}

public class UpdateRecommendationHandler : IRequestHandler<UpdateRecommendationCommand, RecommendationMiniResponseDto?>
{
    private readonly IRecommendationRepository _recommendationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateRecommendationHandler> _logger;

    public UpdateRecommendationHandler(
        IRecommendationRepository recommendationRepository,
        IMapper mapper,
        ILogger<UpdateRecommendationHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _recommendationRepository = recommendationRepository;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<RecommendationMiniResponseDto?> Handle(UpdateRecommendationCommand request, CancellationToken cancellationToken)
    {
        var recommendation = await _recommendationRepository.GetByIdAsync(request.Id, cancellationToken);

        if (recommendation == null) 
            throw GrpcExceptions.NotFound($"Recommendation", request.Id);

        _mapper.Map(request, recommendation);

        recommendation.JsonContent = JsonSerializer.Serialize(request.JsonContent);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RecommendationMiniResponseDto>(recommendation);
    }
}