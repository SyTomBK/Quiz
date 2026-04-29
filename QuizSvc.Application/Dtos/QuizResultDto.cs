namespace QuizSvc.Application.Dtos;

public class QuizResultMiniResponseDto
{
    public Guid Id { get; set; }
    public Guid QuizAttemptId { get; set; }
    public decimal TotalScore { get; set; }
    public List<DimensionScoreResultDto>? DimensionScoreResults { get; set; }
}

public class QuizResultResponseDto
{
    public Guid Id { get; set; }
    public Guid QuizAttemptId { get; set; }
    public Guid? LeadId { get; set; }
    public Guid? UserId { get; set; }
    public Guid QuizId { get; set; }
    public required string QuizTitle { get; set; }
    public decimal TotalScore { get; set; }
    public List<DimensionScoreResultDto>? DimensionScoreResults { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}


public class DimensionScoreResultDto
{
    public Guid DimensionId { get; set; }
    public string DimensionTitle { get; set; } = default!;
    public decimal Score { get; set; }
}

public class CampaignQuizResultResponseDto
{
    public Guid Id { get; set; }
    public string? LeadName { get; set; }
    public string? Username { get; set; }
    public Guid QuizAttemptId { get; set; }
    public string? CampaignName { get; set; }
    public string? QuizTitle { get; set; }
    public decimal TotalScore { get; set; }
    public required string Tenant { get; set; }
    public List<DimensionScoreResultDto>? DimensionScoreResults { get; set; }
    public DateTime CreatedAt { get; set; }
}
