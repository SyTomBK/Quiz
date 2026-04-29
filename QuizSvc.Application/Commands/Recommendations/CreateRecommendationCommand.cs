using QuizSvc.Application.Contracts.Persistence;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.Recommendations;

public class CreateRecommendationCommand : IRequest<RecommendationMiniResponseDto>
{
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

public class CreateRecommendationHandler : IRequestHandler<CreateRecommendationCommand, RecommendationMiniResponseDto>
{
    private readonly IRecommendationRepository _recommendationRepository;
    private readonly ICampaignRepository _campaignRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IDimensionRepository _dimensionRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRecommendationHandler(
        IRecommendationRepository recommendationRepository, 
        IMapper mapper,
        ICampaignRepository campaignRepository,
        IQuizRepository quizRepository,
        IDimensionRepository dimensionRepository,
        IUnitOfWork unitOfWork)
    {
        _recommendationRepository = recommendationRepository;
        _campaignRepository = campaignRepository;
        _quizRepository = quizRepository;
        _dimensionRepository = dimensionRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<RecommendationMiniResponseDto> Handle(CreateRecommendationCommand request, CancellationToken cancellationToken)
    {

        var recommendation = _mapper.Map<Recommendation>(request);

        if (request.CampaignId != null && request.CampaignId != Guid.Empty)
           await _campaignRepository.EnsureCampainExist((Guid)request.CampaignId, cancellationToken);

        if (request.QuizId != null && request.QuizId != Guid.Empty)
            await _quizRepository.EnsureQuizExist((Guid)request.QuizId, cancellationToken);

        if (request.DimensionId != null && request.DimensionId != Guid.Empty)
            await _dimensionRepository.EnsureDimensionExist((Guid)request.DimensionId, cancellationToken);

        await _recommendationRepository.AddAsync(recommendation, cancellationToken);
            
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RecommendationMiniResponseDto>(recommendation);
    }
}
