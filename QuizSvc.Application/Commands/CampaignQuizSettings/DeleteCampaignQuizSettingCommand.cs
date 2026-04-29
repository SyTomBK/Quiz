
using QuizSvc.Application.Common.Exceptions;

namespace QuizSvc.Application.Commands.CampaignQuizSettings;

public class DeleteCampaignQuizSettingCommand : IRequest
{
    public Guid Id { get; set; }
}
public class DeleteCampaignQuizSettingCommandHandler : IRequestHandler<DeleteCampaignQuizSettingCommand>
{
    private readonly IMapper _mapper;
    private readonly ICampaignQuizSettingRepository _campaignQuizSettingRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly ICampaignRepository _campaignRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteCampaignQuizSettingCommandHandler(
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
    public async Task Handle(DeleteCampaignQuizSettingCommand request, CancellationToken cancellationToken)
    {
        var count = await _campaignQuizSettingRepository.DeleteByIdAsync(request.Id, cancellationToken);

        if(count == 0)
            throw GrpcExceptions.NotFound("CampaignQuizSetting", request.Id);

    }
}
