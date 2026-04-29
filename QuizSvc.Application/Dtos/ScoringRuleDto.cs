namespace QuizSvc.Application.Dtos;

public class ScoringRuleMiniResponseDto
{
    public required Guid Id { get; set; }
    public required string Tenant { get; set; }
    public required string RuleKey { get; set; }
    public string? Description { get; set; }
    public double Points { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class ScoringRuleDetailResponseDto
{
    public required Guid Id { get; set; }
    public required string Tenant { get; set; }
    public required string RuleKey { get; set; }
    public string? Description { get; set; }
    public double Points { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}