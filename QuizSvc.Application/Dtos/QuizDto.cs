namespace QuizSvc.Application.Dtos;
public class QuizMiniResponseDto
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Instruction { get; set; }
    public string? Image { get; set; }
    public QuizType Type { get; set; }
    public required string Tenant { get; set; }
    public Guid? ParentId { get; set; }
    public QuizSource Source { get; set; }
    public decimal EstimateTime { get; set; }
    public int SubmitedAmount { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
}

public class QuizResponseDto
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Instruction { get; set; }
    public string? Image { get; set; }
    public QuizType Type { get; set; }
    public required string Tenant { get; set; }
    public Guid? ParentId { get; set; }
    public QuizSource Source { get; set; }
    public decimal EstimateTime { get; set; }
    public List<DimensionMiniResponseDto> Dimensions { get; set; } = new List<DimensionMiniResponseDto>();
    public List<QuestionMiniResponseDto> Questions { get; set; } = new List<QuestionMiniResponseDto>();
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}
public class GetQuizInCampaignDetailResponseDto
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Instruction { get; set; }
    public string? Image { get; set; }
    public QuizType Type { get; set; }
    public required string Tenant { get; set; }
    public Guid? ParentId { get; set; }
    public QuizSource Source { get; set; }
    public decimal EstimateTime { get; set; }
    public int SubmitedAmount { get; set; } = 0;
    public List<DimensionMiniResponseDto> Dimensions { get; set; } = [];
    public List<QuestionMiniResponseDto> Questions { get; set; } = [];
    public CampaignQuizSettingResponseDto QuizSetting { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}
