using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.Dimensions;

public class UpdateQuizDimensionCommand : IRequest<DimensionMiniResponseDto>
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}
public class UpdateQuizDimensionCommandHandler : IRequestHandler<UpdateQuizDimensionCommand, DimensionMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IDimensionRepository _dimensionRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateQuizDimensionCommandHandler(IMapper mapper,
        IUnitOfWork unitOfWork,
        IDimensionRepository dimensionRepository, 
        IQuizRepository quizRepository)
    {
        _mapper = mapper;
        _dimensionRepository = dimensionRepository;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DimensionMiniResponseDto> Handle(UpdateQuizDimensionCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetByIdAsync(request.QuizId, cancellationToken);

        if (quiz == null)
            throw GrpcExceptions.NotFound("Quiz", request.QuizId);

        var dimension = await _dimensionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (dimension == null || dimension.QuizId != request.QuizId)
            throw GrpcExceptions.NotFound("Dimension", request.Id);

         dimension.Title = request.Title;

         dimension.Description = request.Description;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<DimensionMiniResponseDto>(dimension);
    }
}
