using QuizSvc.Application.Dtos;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Queries.Dimensions;

public class GetQuizDimensionListQuery : IRequest<PagedList<QuizDimensionMiniResponseDto>>
{
    public string? Title { get; set; }
    public required string Tenant { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}
public class GetQuizDimensionListQueryHandler : IRequestHandler<GetQuizDimensionListQuery, PagedList<QuizDimensionMiniResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IDimensionRepository _dimensionRepository;
    public GetQuizDimensionListQueryHandler(IMapper mapper, IDimensionRepository dimensionRepository)
    {
        _mapper = mapper;
        _dimensionRepository = dimensionRepository;
    }
    public async Task<PagedList<QuizDimensionMiniResponseDto>> Handle(GetQuizDimensionListQuery request, CancellationToken cancellationToken)
    {
        var result = await _dimensionRepository.GetQuizDimensionList(request, cancellationToken);
        return result;
    }
}
