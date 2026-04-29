using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.Quizs;
public class GetQuizInCampaignDetailQuery : IRequest<GetQuizInCampaignDetailResponseDto>
{
    public Guid QuizId { get; set; }
    public Guid CampaignId { get; set; }
}
public class GetQuizInCampaignDetailQueryHandler : IRequestHandler<GetQuizInCampaignDetailQuery, GetQuizInCampaignDetailResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IQuizRepository _quizRepository;
    public GetQuizInCampaignDetailQueryHandler(IMapper mapper, IQuizRepository quizRepository)
    {
        _mapper = mapper;
        _quizRepository = quizRepository;
    }
    public async Task<GetQuizInCampaignDetailResponseDto> Handle(GetQuizInCampaignDetailQuery request, CancellationToken cancellationToken)
    {
        var quiz = await _quizRepository.GetQuizInCampaignDetail(request.QuizId, request.CampaignId, cancellationToken);
        return quiz;
    }
}
