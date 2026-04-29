using QuizSvc.Application.Common.Exceptions;

namespace QuizSvc.Application.Commands.Questions;

public class DeletedQuestionCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeletedQuestionCommandHandler : IRequestHandler<DeletedQuestionCommand>
{
    private readonly IMapper _mapper;
    private readonly IQuestionRepository _questionRepository;
    public DeletedQuestionCommandHandler(IMapper mapper, IQuestionRepository questionRepository)
    {
        _mapper = mapper;
        _questionRepository = questionRepository;
    }
    public async Task Handle(DeletedQuestionCommand request, CancellationToken cancellationToken)
    {
        var count = await _questionRepository.DeleteByIdAsync(request.Id, cancellationToken);

        if(count == 0)
            throw GrpcExceptions.NotFound("Question", request.Id);
    }
}