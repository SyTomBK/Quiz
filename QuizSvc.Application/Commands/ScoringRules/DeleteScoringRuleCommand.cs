using QuizSvc.Application.Common.Exceptions;
namespace QuizSvc.Application.Commands.ScoringRules;

public class DeleteScoringRuleCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteScoringRuleCommandHandler : IRequestHandler<DeleteScoringRuleCommand>
{
    private readonly IMapper _mapper;
    private readonly IScoringRuleRepository _scoringRuleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteScoringRuleCommandHandler(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IScoringRuleRepository scoringRuleRepository
    )
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _scoringRuleRepository = scoringRuleRepository;
    }
    public async Task Handle(DeleteScoringRuleCommand request, CancellationToken cancellationToken)
    {
        var count = await _scoringRuleRepository.DeleteByIdAsync(request.Id, cancellationToken);

        if (count == 0)
            throw GrpcExceptions.NotFound("ScoringRule", request.Id);
    }
}
