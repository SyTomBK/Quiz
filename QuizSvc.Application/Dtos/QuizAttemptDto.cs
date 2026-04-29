namespace QuizSvc.Application.Dtos;

public class QuizAttemptMiniResponseDto
{
    public Guid Id { get; set; }
    public Guid CampaignQuizSettingId { get; set; }
    public Guid? LeadId { get; set; }
    public Guid? UserId { get; set; }
}

public class UserAnswerRequestDto
{
    public Guid QuestionId { get; set; }
    public Guid AnswerId { get; set; }
}

public class UserAnswerResponseDto
{
    public Guid QuestionId { get; set; }
    public Guid AnswerId { get; set; }
}

public class QuizAttemptResponseDto
{
    public Guid Id { get; set; }
    public Guid CampaignQuizSettingId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? LeadId { get; set; }
    public AttemptStatus AttemptStatus { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? CurrentQuestionId { get; set; }
    public List<UserAnswerResponseDto> UserAnswers { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}


