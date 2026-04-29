namespace QuizSvc.Application.Dtos;

public class LeadEngagementResponseDto
{
    public Guid LeadId { get; set; }
    public double OverallEngagementScore { get; set; }
    public DateTime LastCalculatedAt { get; set; }
    public List<InterestAffinityDto>? InterestAffinities { get; set; }
}

public class InterestAffinityDto
{
    public Guid DimensionId { get; set; }
    public string? DimensionName { get; set; }
    public double Score { get; set; }
}

public class ScoringRuleDto
{
    public Guid Id { get; set; }
    public string Tenant { get; set; } = default!;
    public string RuleKey { get; set; } = default!;
    public string? Description { get; set; }
    public double Points { get; set; }
    public bool IsActive { get; set; }
}

public class InteractionLogMiniResponseDto
{
    public Guid Id { get; set; }
    public Guid LeadId { get; set; }
    public string Tenant { get; set; } = default!;
    public Guid? QuizAttemptId { get; set; }
    public string EventType { get; set; } = default!;
    public Guid? TargetId { get; set; }
    public double Value { get; set; }
    public double Score { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PagedInteractionLogMiniResponseDto
{
    public List<InteractionLogMiniResponseDto> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
