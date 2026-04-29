namespace QuizSvc.Application.Dtos;

public class CreatedDimensionScoreRequestDto
{
    public required string TempDimId { get; set; }
    public required decimal Score { get; set; }
}

public class CreatedQuizDimensionScoreRequestDto
{
    public required Guid DimensionId { get; set; }
    public required decimal Score { get; set; }
}

public class DimensionScoreMiniResponseDto
{
    public required Guid Id { get; set; }
    public required Guid DimensionId { get; set; }
    public required string Title { get; set; }
    public required decimal Score { get; set; }
}

public class UpdatedDimensionScoreRequestDto
{
    public required Guid DimensionId { get; set; }
    public required decimal Score { get; set; }
}