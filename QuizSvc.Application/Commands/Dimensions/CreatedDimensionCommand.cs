using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.Dimensions;

public class CreatedDimensionCommand : IRequest<DimensionMiniResponseDto>
{
    public required Guid QuizId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}

public class CreatedDimensionCommandHandler : IRequestHandler<CreatedDimensionCommand, DimensionMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IDimensionRepository _dimensionRepository;
    private readonly IQuizRepository _quizRepository;
    public CreatedDimensionCommandHandler(IMapper mapper, IDimensionRepository dimensionRepository, IQuizRepository quizRepository)
    {
        _mapper = mapper;
        _dimensionRepository = dimensionRepository;
        _quizRepository = quizRepository;
    }

    public async Task<DimensionMiniResponseDto> Handle(CreatedDimensionCommand request, CancellationToken cancellationToken)
    {
        await _quizRepository.EnsureQuizExist(request.QuizId, cancellationToken);
        var resutl = await _dimensionRepository.CreateDimention(request, cancellationToken);
        return resutl;
    }
}
