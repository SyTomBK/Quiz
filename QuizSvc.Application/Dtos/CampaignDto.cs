namespace QuizSvc.Application.Dtos;

public class CampaignMiniResponseDto
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; } = default!;
    public required string Address { get; set; }
    public CampaignStatus Status { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CampaignResponseDto
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Address { get; set; }
    public string? Image { get; set; }
    public CampaignStatus Status { get; set; }
    public required string Tenant { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public List<CampaignQuizSettingResponseDto> QuizSettings { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}