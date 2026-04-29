using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Contracts.Persistence;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.CampaignQuizSettings;

public class UpdateCampaignQuizSettingCommand : IRequest<CampaignQuizSettingMiniResponseDto>
{
    public required Guid Id { get; set; }
    public required Guid QuizId { get; set; }
    public bool IsActive { get; set; } = true;
    public int MaxAttempts { get; set; }
    public LeadCollectionPolicyDto? LeadCollectionPolicy { get; set; }
    public CheckpointConfigDto? CheckpointConfig { get; set; }
}

public class UpdateCampaignQuizSettingCommandHandler : IRequestHandler<UpdateCampaignQuizSettingCommand, CampaignQuizSettingMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly ICampaignQuizSettingRepository _campaignQuizSettingRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateCampaignQuizSettingCommandHandler(
        IMapper mapper,
        IQuizRepository quizRepository,
        ICampaignQuizSettingRepository campaignQuizSettingRepository,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
        _campaignQuizSettingRepository = campaignQuizSettingRepository;
    }
    public async Task<CampaignQuizSettingMiniResponseDto> Handle(UpdateCampaignQuizSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = await _campaignQuizSettingRepository.GetByIdAsync(request.Id, cancellationToken);
       
        setting.IsActive = request.IsActive;

        setting.MaxAttempts = request.MaxAttempts;

        await _quizRepository.EnsureQuizExist(request.QuizId, cancellationToken);

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

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CampaignQuizSettingMiniResponseDto>(setting);
    }
}
