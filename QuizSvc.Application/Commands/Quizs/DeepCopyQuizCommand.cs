using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Contracts.Persistence;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Services.Quizes;

namespace QuizSvc.Application.Commands.Quizs;

public class DeepCopyQuizCommand : IRequest<QuizMiniResponseDto>
{
    public required Guid QuizTemplateId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Instruction { get; set; }
    public string? Image { get; set; }
    public required string Tenant { get; set; }
    public decimal EstimateTime { get; set; }
}

public class DeepCopyQuizCommandHandler : IRequestHandler<DeepCopyQuizCommand, QuizMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IQuizService _quizService;
    public DeepCopyQuizCommandHandler(IMapper mapper, IQuizRepository quizRepository, IUnitOfWork unitOfWork, IQuizService quizService)
    {
        _mapper = mapper;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
        _quizService = quizService;
    }
    public async Task<QuizMiniResponseDto> Handle(DeepCopyQuizCommand request, CancellationToken cancellationToken)
    {
        var template = await _quizRepository
          .GetTemplateFullGraph(request.QuizTemplateId, cancellationToken);

        if (template == null)
            throw GrpcExceptions.NotFound("Quiz Template", request.QuizTemplateId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var newQuiz = _quizService.DeepCopy(template, request);

            await _quizRepository.AddAsync(newQuiz, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<QuizMiniResponseDto>(newQuiz);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw GrpcExceptions.Internal($"Error when deep copying quiz: {ex.Message}");
        }
    }
}