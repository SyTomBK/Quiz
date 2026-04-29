using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Queries.Dimensions;

public class GetDimensionDetailQuery : IRequest<DimensionResponseDto>
{
    public Guid Id { get; set; }
}

public class GetDimensionDetailQueryHandler : IRequestHandler<GetDimensionDetailQuery, DimensionResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IDimensionRepository _dimensionRepository;
    public GetDimensionDetailQueryHandler(IMapper mapper, IDimensionRepository dimensionRepository)
    {
        _mapper = mapper;
        _dimensionRepository = dimensionRepository;
    }
    public async Task<DimensionResponseDto> Handle(GetDimensionDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dimensionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            throw GrpcExceptions.NotFound("Dimension", request.Id);

        return _mapper.Map<DimensionResponseDto>(entity);
    }
}
