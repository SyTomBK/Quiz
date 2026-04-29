using QuizSvc.Application.Dtos;
using QuizSvc.Application.Services.Quizes;

namespace QuizSvc.Application.Commands.Quizs;
public class CreatedQuizCommand : IRequest<QuizMiniResponseDto>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Instruction { get; set; }
    public string? Image { get; set; }
    public QuizType Type { get; set; }
    public string? Tenant { get; set; }
    public Guid? ParentId { get; set; }
    public QuizSource Source { get; set; }
    public decimal EstimateTime { get; set; }
    public List<CreatedDimensionRequestDto> Dimensions { get; set; } = new();
    public List<CreatedQuestionRequestDto> Questions { get; set; } = new();
}

public class CreatedQuizCommandHandler : IRequestHandler<CreatedQuizCommand, QuizMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizService _quizService;
    public CreatedQuizCommandHandler(IMapper mapper, IQuizService quizService)
    {
        _mapper = mapper;
        _quizService = quizService;
    }

    public async Task<QuizMiniResponseDto> Handle(CreatedQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _quizService.CreateQuiz(request, cancellationToken);
        return _mapper.Map<QuizMiniResponseDto>(quiz);
    }
}
