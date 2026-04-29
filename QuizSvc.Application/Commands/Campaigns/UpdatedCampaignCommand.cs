using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.Campaigns;
public class UpdatedCampaignCommand : IRequest<CampaignMiniResponseDto>
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Address { get; set; }
    public string? Image { get; set; }
    public CampaignStatus Status { get; set; }
    public required string Tenant { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public List<Guid> QuizSettingDeletedIds { get; set; } = [];
    public List<CampaignQuizSettingDto> QuizSettingCreateds { get; set; } = [];
    public List<UpdatedCampaignQuizSettingDto> QuizSettingUpdateds { get; set; } = [];
}

public class UpdatedCampaignCommandHandler : IRequestHandler<UpdatedCampaignCommand, CampaignMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly ICampaignRepository _campaignRepository;
    private readonly ICampaignQuizSettingRepository _campaignQuizSettingRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatedCampaignCommandHandler(IMapper mapper,
        ICampaignQuizSettingRepository campaignQuizSettingRepository,
        ICampaignRepository campaignRepository,
        IUnitOfWork unitOfWork,
        IQuizRepository quizRepository)
    {
        _mapper = mapper;
        _campaignRepository = campaignRepository;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
        _campaignQuizSettingRepository = campaignQuizSettingRepository;
    }

    public async Task<CampaignMiniResponseDto> Handle(UpdatedCampaignCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        await ValidateCreated(request, cancellationToken);
        await ValidateUpdated(request, cancellationToken);
        await ValidateDeleted(request, cancellationToken);

        var campaign = await _campaignRepository.GetCampainByIdAsync(request.Id, cancellationToken);
        
        if(campaign == null)
            throw GrpcExceptions.NotFound("Campaign", request.Id);

        _mapper.Map(request, campaign);

        HandleCreated(request, campaign);
        await HandleUpdated(request, cancellationToken);
        await HandleDeleted(request, cancellationToken);

        var result = _mapper.Map<CampaignMiniResponseDto>(campaign);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return result;
    }

    private async Task ValidateCreated(UpdatedCampaignCommand request, CancellationToken cancellationToken)
    {
        if (request.QuizSettingCreateds.Count == 0) return;

        await ValidateQuizIdsAsync(request.QuizSettingCreateds.Select(x => x.QuizId),"Created Quiz", cancellationToken);
    }

    private async Task ValidateUpdated(UpdatedCampaignCommand request, CancellationToken cancellationToken)
    {
        if (request.QuizSettingUpdateds.Count == 0) return;

        await ValidateQuizIdsAsync(
            request.QuizSettingUpdateds.Select(x => x.QuizId),"Updated Quiz", cancellationToken);
    }

    private async Task ValidateDeleted(UpdatedCampaignCommand request, CancellationToken cancellationToken)
    {
        if (request.QuizSettingDeletedIds.Count == 0) return;

        await ValidateQuizIdsAsync(
            request.QuizSettingDeletedIds, "Deleted Quiz", cancellationToken);
    }

    private async Task ValidateQuizIdsAsync(IEnumerable<Guid> quizIds, string entityName, CancellationToken cancellationToken)
    {
        var list = quizIds.ToList();

        if (list.Count != list.Distinct().Count())
            throw GrpcExceptions.BadRequest($"{entityName} bị trùng trong campaign");

        var existing = await _quizRepository.GetExistingIds(list, cancellationToken);
        var missing = list.Except(existing);

        if (missing.Any())
            throw GrpcExceptions.NotFound("Quiz", missing.First());
    }

    private void HandleCreated(UpdatedCampaignCommand request, Campaign campaign)
    {
        foreach (var quiz in request.QuizSettingCreateds)
        {
            var entity = new CampaignQuizSetting
            {
                Id = Guid.NewGuid(),
                CampaignId = campaign.Id,
                QuizId = quiz.QuizId,
                IsActive = true,
                MaxAttempts = quiz.MaxAttempts,
                LeadCollectionPolicy = quiz.LeadCollectionPolicy == null
                    ? null : _mapper.Map<LeadCollectionPolicy>(quiz.LeadCollectionPolicy),
                CheckpointConfig = quiz.CheckpointConfig == null
                    ? null : _mapper.Map<CheckpointConfig>(quiz.CheckpointConfig),
            };

            campaign.CampaignQuizSettings.Add(entity);
        }
    }

    private async Task HandleUpdated(UpdatedCampaignCommand request, CancellationToken cancellationToken)
    {
        if (request.QuizSettingUpdateds.Count == 0) return;

        var ids = request.QuizSettingUpdateds.Select(x => x.Id).ToList();

        var settings = await _campaignQuizSettingRepository.GetByIdsAsync(ids, cancellationToken);

        if(settings.Count != ids.Count) 
            throw GrpcExceptions.NotFound("CampaignQuizSetting", "One or more ids not found for update");

        var map = settings.ToDictionary(x => x.Id);

        foreach (var dto in request.QuizSettingUpdateds)
        {
            if (!map.TryGetValue(dto.Id, out var setting))
                throw GrpcExceptions.NotFound("CampaignQuizSetting", dto.Id);

            setting.QuizId = dto.QuizId;
            setting.MaxAttempts = dto.MaxAttempts;

            if (dto.LeadCollectionPolicy != null)
                setting.LeadCollectionPolicy =
                    _mapper.Map<LeadCollectionPolicy>(dto.LeadCollectionPolicy);

            if (dto.CheckpointConfig != null)
                setting.CheckpointConfig =
                    _mapper.Map<CheckpointConfig>(dto.CheckpointConfig);
        }
    }

    private async Task HandleDeleted( UpdatedCampaignCommand request, CancellationToken cancellationToken)
    {
        if (request.QuizSettingDeletedIds.Count == 0) return;

        await _campaignQuizSettingRepository
            .DeletedCampainQuizSettingIdsAsync(request.QuizSettingDeletedIds,cancellationToken);
    }
}