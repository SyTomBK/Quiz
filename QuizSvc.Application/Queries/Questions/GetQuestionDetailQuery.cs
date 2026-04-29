using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.Questions;

public class GetQuestionDetailQuery : IRequest<QuestionMiniResponseDto>
{
    public Guid Id { get; set; }
}

public class GetQuestionDetailQueryHandler : IRequestHandler<GetQuestionDetailQuery, QuestionMiniResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuestionRepository _questionRepository;

    public GetQuestionDetailQueryHandler(IMapper mapper, IQuestionRepository questionRepository)
    {
        _mapper = mapper;
        _questionRepository = questionRepository;
    }

    public async Task<QuestionMiniResponseDto> Handle(GetQuestionDetailQuery request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetWithIncludeByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<QuestionMiniResponseDto>(question);
    }
}