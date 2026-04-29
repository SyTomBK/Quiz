using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.Quizs;

public class UpdateQuizCommand : IRequest<QuizMiniResponseDto>
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Instruction { get; set; }
    public string? Image { get; set; }
    public decimal EstimateTime { get; set; }
}

public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, QuizMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateQuizCommandHandler(IMapper mapper, IQuizRepository quizRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<QuizMiniResponseDto> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
    {
        var entity = await _quizRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            throw GrpcExceptions.NotFound("Quiz", request.Id);

        _mapper.Map(request, entity);

        _ = _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<QuizMiniResponseDto>(entity);
    }
}
