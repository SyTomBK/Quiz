using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.Campaigns;
public class CreatedCampaignCommand : IRequest<CampaignMiniResponseDto>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Address { get; set; }
    public string? Image { get; set; }
    public CampaignStatus Status { get; set; }
    public required string Tenant { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public List<CampaignQuizSettingDto> QuizSettings { get; set; } = new();
}

public class CreatedCampaignCommandHandler : IRequestHandler<CreatedCampaignCommand, CampaignMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly ICampaignRepository _campaignRepository;
    private readonly IQuizRepository _quizRepository;
    public CreatedCampaignCommandHandler(IMapper mapper, ICampaignRepository campaignRepository, IQuizRepository quizRepository)
    {
        _mapper = mapper;
        _campaignRepository = campaignRepository;
        _quizRepository = quizRepository;
    }
    public async Task<CampaignMiniResponseDto> Handle(CreatedCampaignCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate duplicate quiz
        var duplicateQuiz = request.QuizSettings
           .Select(x => x.QuizId)
           .Distinct()
           .Count() != request.QuizSettings.Count;

        if (duplicateQuiz)
            throw GrpcExceptions.BadRequest("Quiz bị trùng trong campaign");

        // 2. Validate quiz tồn tại
        var quizIds = request.QuizSettings.Select(x => x.QuizId).ToList();
        var existingQuizIds = await _quizRepository
           .GetExistingIds(quizIds, cancellationToken);

        var missingQuiz = quizIds.Except(existingQuizIds);

        if (missingQuiz.Any())
            throw GrpcExceptions.NotFound("Quiz", missingQuiz.First());

        // 3. Build Campaign (Aggregate Root)
        var campaign = new Campaign
        {
            Id = Guid.NewGuid(),
            Code = "CMP_" + Guid.NewGuid().ToString("N")[..6],
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            Image = request.Image,
            Status = request.Status,
            Tenant = request.Tenant,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        };

        // 4. Build child entities
        foreach (var quizSetting in request.QuizSettings)
        {
            var setting = new CampaignQuizSetting
            {
                Id = Guid.NewGuid(),
                CampaignId = campaign.Id,
                QuizId = quizSetting.QuizId,
                IsActive = true,
                MaxAttempts = quizSetting.MaxAttempts,
            };

            if (quizSetting.LeadCollectionPolicy != null)
            {
                setting.LeadCollectionPolicy =
                    _mapper.Map<LeadCollectionPolicy>(quizSetting.LeadCollectionPolicy);
            }

            if (quizSetting.CheckpointConfig != null)
            {
                setting.CheckpointConfig =
                    _mapper.Map<CheckpointConfig>(quizSetting.CheckpointConfig);
            }

            campaign.CampaignQuizSettings.Add(setting);
        }

        // 5. Persist
        await _campaignRepository.AddAsync(campaign, cancellationToken);

        // 6. Return DTO
        return _mapper.Map<CampaignMiniResponseDto>(campaign);
    }
}
