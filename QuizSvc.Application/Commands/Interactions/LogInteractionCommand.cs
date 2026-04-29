namespace QuizSvc.Application.Commands.Interactions;

public class LogInteractionCommand : IRequest<Unit>
{
    public Guid LeadId { get; set; }
    public string Tenant { get; set; } = default!;
    public Guid? QuizAttemptId { get; set; }
    public string EventType { get; set; } = default!;
    public Guid? TargetId { get; set; }
    public double Value { get; set; }
    public string Description { get; set; } = default!;
}

public class LogInteractionCommandHandler : IRequestHandler<LogInteractionCommand, Unit>
{
    private readonly IInteractionRepository _interactionRepository;
    private readonly IMapper _mapper;

    public LogInteractionCommandHandler(IInteractionRepository interactionRepository, IMapper mapper)
    {
        _interactionRepository = interactionRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(LogInteractionCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra xem đã có log tương tự trong vòng 1 giây qua chưa
        var isDuplicate = await _interactionRepository.IsDuplicateAsync(request.LeadId, request.EventType, 1, cancellationToken);
        if (isDuplicate)
            return Unit.Value;

        var log = _mapper.Map<InteractionLog>(request);

        log.IsProcessed = false;

        await _interactionRepository.LogInteraction(log, cancellationToken);

        return Unit.Value;
    }
}
