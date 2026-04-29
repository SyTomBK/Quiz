namespace QuizSvc.Application.Dtos;

public class CreatedAnswerRequestDto
{
    public required string Content { get; set; }
    public required bool IsCorrect { get; set; }
    public required decimal Score { get; set; }
    public List<CreatedDimensionScoreRequestDto> DimensionScores { get; set; } = new List<CreatedDimensionScoreRequestDto>();
}

public class AnswerMiniResponseDto
{
    public required Guid Id { get; set; }
    public required string Content { get; set; }
    public required bool IsCorrect { get; set; }
    public required decimal Score { get; set; }
    public List<DimensionScoreMiniResponseDto> DimensionScores { get; set; } = new List<DimensionScoreMiniResponseDto>();
}

public class UpdatedAnswerRequestDto
{
    public required Guid Id { get; set; }
    public required string Content { get; set; }
    public required bool IsCorrect { get; set; }
    public required decimal Score { get; set; }
    public List<UpdatedDimensionScoreRequestDto> DimensionScores { get; set; } = new List<UpdatedDimensionScoreRequestDto>();
}

public class CreatedQuizAnswerRequestDto
{
    public required string Content { get; set; }
    public required bool IsCorrect { get; set; }
    public required decimal Score { get; set; }
    public List<CreatedQuizDimensionScoreRequestDto> DimensionScores { get; set; } = [];
}