using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Contracts.Persistence;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.Quizs;

public class CloneQuizCommand : IRequest<QuizMiniResponseDto>
{
    public required Guid QuizTemplateId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Instruction { get; set; }
    public string? Image { get; set; }
    public required string Tenant { get; set; }
    public decimal EstimateTime { get; set; }
}
public class CloneQuizCommandHandler : IRequestHandler<CloneQuizCommand, QuizMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICodeGenerator _codeGenerator;
    public CloneQuizCommandHandler(IMapper mapper, IQuizRepository quizRepository,
        IUnitOfWork unitOfWork, ICodeGenerator codeGenerator)
    {
        _mapper = mapper;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
        _codeGenerator = codeGenerator;
    }
    public async Task<QuizMiniResponseDto> Handle(CloneQuizCommand request, CancellationToken cancellationToken)
    {
        var templateQuiz = await _quizRepository
            .GetTemplateById(request.QuizTemplateId, cancellationToken);

        if (templateQuiz == null)
            throw GrpcExceptions.NotFound("Quiz Template", request.QuizTemplateId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var newQuiz = _mapper.Map<Domain.Entities.Quiz>(request);

            newQuiz.Id = Guid.NewGuid();
            newQuiz.Source = QuizSource.Refference;
            newQuiz.Type = templateQuiz.Type;
            newQuiz.ParentId = templateQuiz.Id;
            newQuiz.Code = _codeGenerator.Generate("QIZ");

            await _quizRepository.AddAsync(newQuiz, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return _mapper.Map<QuizMiniResponseDto>(newQuiz);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw new Exception($"Clone quiz failed: {ex.Message}");
        }
    }
}
