using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.CampaignQuizSettings;

public class CreateCampaignQuizSettingCommand : IRequest<CampaignQuizSettingMiniResponseDto>
{
    public Guid CampaignId { get; set; }
    public Guid QuizId { get; set; }
    public bool IsActive { get; set; } = true;
    public int MaxAttempts { get; set; }
    public LeadCollectionPolicyDto? LeadCollectionPolicy { get; set; }
    public CheckpointConfigDto? CheckpointConfig { get; set; }
}

public class CreateCampaignQuizSettingCommandHandler : IRequestHandler<CreateCampaignQuizSettingCommand, CampaignQuizSettingMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly ICampaignQuizSettingRepository _campaignQuizSettingRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly ICampaignRepository _campaignRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCampaignQuizSettingCommandHandler(
        IMapper mapper,
        IQuizRepository quizRepository,
        ICampaignRepository campaignRepository,
        ICampaignQuizSettingRepository campaignQuizSettingRepository,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _quizRepository = quizRepository;
        _campaignRepository = campaignRepository;
        _unitOfWork = unitOfWork;
        _campaignQuizSettingRepository = campaignQuizSettingRepository;
    }
    public async Task<CampaignQuizSettingMiniResponseDto> Handle(CreateCampaignQuizSettingCommand request, CancellationToken cancellationToken)
    {
        var exist = await _campaignQuizSettingRepository.ExistByCampaignAndQuizAsync(request.CampaignId, request.QuizId, cancellationToken);

        if(exist)
            throw GrpcExceptions.AlreadyExists($"CampaignQuizSetting with CampaignId {request.CampaignId} and QuizId {request.QuizId} already exists.");

        await _quizRepository.EnsureQuizExist(request.QuizId, cancellationToken);
        await _campaignRepository.EnsureCampainExist(request.CampaignId, cancellationToken);

        var setting = new CampaignQuizSetting();

        setting.MaxAttempts = request.MaxAttempts;
        setting.IsActive = request.IsActive;
        setting.CampaignId = request.CampaignId;
        setting.QuizId = request.QuizId;

        if (request.LeadCollectionPolicy != null)
        {
            setting.LeadCollectionPolicy =
                _mapper.Map<LeadCollectionPolicy>(request.LeadCollectionPolicy);
        }

        if (request.CheckpointConfig != null)
        {
            setting.CheckpointConfig =
                _mapper.Map<CheckpointConfig>(request.CheckpointConfig);
        }

        await _campaignQuizSettingRepository.AddAsync(setting, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CampaignQuizSettingMiniResponseDto>(setting);
    }
}
