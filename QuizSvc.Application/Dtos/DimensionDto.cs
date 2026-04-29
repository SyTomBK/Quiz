namespace QuizSvc.Application.Dtos;

public class CreatedDimensionRequestDto
{
    public required string Title { get; set; }
    public required string TempDimId { get; set; }
    public string? Description { get; set; }
}

public class DimensionMiniResponseDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}

public class UpdatedDimensionRequestDto
{
    public required string Title { get; set; }
    public Guid? Id { get; set; }
    public string? Description { get; set; }
}

public class DimensionResponseDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}
public class QuizDimensionMiniResponseDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required Guid QuizId { get; set; }
    public required string QuizTitle { get; set; }
}