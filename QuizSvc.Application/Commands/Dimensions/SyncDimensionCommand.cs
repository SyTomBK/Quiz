using QuizSvc.Application.Common.Exceptions;
using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Commands.Dimensions;

public class SyncDimensionCommand : IRequest
{
    public required Guid QuizId { get; set; }
    public required List<UpdatedDimensionRequestDto> Dimensions { get; set; }
}

public class SyncDimensionCommandHandler : IRequestHandler<SyncDimensionCommand>
{
    private readonly IMapper _mapper;
    private readonly IDimensionRepository _dimensionRepository;
    private readonly IQuizRepository _quizRepository;
    private readonly IUnitOfWork _unitOfWork;
    public SyncDimensionCommandHandler(IMapper mapper, IDimensionRepository dimensionRepository, 
        IQuizRepository quizRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _dimensionRepository = dimensionRepository;
        _quizRepository = quizRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SyncDimensionCommand request, CancellationToken cancellationToken)
    {
        await _quizRepository.EnsureQuizExist(request.QuizId, cancellationToken);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Load data
            var existingDimensions = await _dimensionRepository.GetByQuizId(request.QuizId, cancellationToken);

            var existingDict = existingDimensions.ToDictionary(x => x.Id);

            // 2.Extract request ids
            var requestIds = request.Dimensions
                .Where(x => x.Id.HasValue && x.Id != Guid.Empty)
                .Select(x => x.Id!.Value)
                .ToHashSet();

            // 3. VALIDATION - check Id tồn tại
            ValidateIds(requestIds, existingDict);

            // 4. Handle delete
            var toRemove = GetDimensionsToRemove(existingDimensions, requestIds);

            if (toRemove.Count != 0)
                _dimensionRepository.RemoveRange(toRemove);
            

            var toAdd = new List<Dimension>();

            foreach (var item in request.Dimensions)
            {
                if (!item.Id.HasValue || item.Id == Guid.Empty)
                {
                    toAdd.Add(new Dimension
                    {
                        Id = Guid.NewGuid(),
                        Title = item.Title,
                        Description = item.Description,
                        QuizId = request.QuizId
                    });
                }
                else
                {
                    if (existingDict.TryGetValue(item.Id.Value, out var existing))
                    {
                        existing.Title = item.Title;
                        existing.Description = item.Description;
                    }
                }
            }

            if (toAdd.Count != 0)
                await _dimensionRepository.AddRangeAsync(toAdd, cancellationToken);
            

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch 
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw ;
        }
    }

    private static void ValidateIds(HashSet<Guid> requestIds, Dictionary<Guid, Dimension> existingDict)
    {
        var invalidIds = requestIds.Except(existingDict.Keys).ToList();

        if (invalidIds.Count > 0)
            throw GrpcExceptions.NotFound($"Các Dimension Id không tồn tại: {string.Join(", ", invalidIds)}");
        
    }

    private static List<Dimension> GetDimensionsToRemove(List<Dimension> existingDimensions, HashSet<Guid> requestIds)
    {
        return existingDimensions.Where(d => !requestIds.Contains(d.Id)).ToList();
    }
}
