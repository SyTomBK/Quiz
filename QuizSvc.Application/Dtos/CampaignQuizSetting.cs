namespace QuizSvc.Application.Dtos;

public class CampaignQuizSettingDto
{
    public Guid QuizId { get; set; }
    public bool IsActive { get; set; } = true;
    public int MaxAttempts { get; set; }
    public LeadCollectionPolicyDto? LeadCollectionPolicy { get; set; }
    public CheckpointConfigDto? CheckpointConfig { get; set; }
}

public class UpdatedCampaignQuizSettingDto
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public bool IsActive { get; set; } = true;
    public int MaxAttempts { get; set; }
    public LeadCollectionPolicyDto? LeadCollectionPolicy { get; set; }
    public CheckpointConfigDto? CheckpointConfig { get; set; }
}
public class CampaignQuizSettingMiniResponseDto
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public bool IsActive { get; set; } = true;
    public int MaxAttempts { get; set; }
}

public class CampaignQuizSettingResponseDto
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public bool IsActive { get; set; } = true;
    public int MaxAttempts { get; set; }
    public LeadCollectionPolicyDto? LeadCollectionPolicy { get; set; }
    public CheckpointConfigDto? CheckpointConfig { get; set; }
}

public class LeadCollectionPolicyDto
{
    public List<LeadField> RequiredFields { get; set; } = new List<LeadField>();
}

public class QuizCheckpointDto
{
    public required string Key { get; set; }
    public required CheckpointTriggerDto Trigger { get; set; }
    public required CheckpointContentDto Content { get; set; }
}

public class CheckpointTriggerDto
{
    public CheckpointTriggerType CheckpointTriggerType { get; set; }
    public int? QuestionNumber { get; set; }
    public int? Interval { get; set; }
    public double? ProgressPercent { get; set; }
}


public class CheckpointContentDto
{
    public CheckpointContentType CheckpointContentType { get; set; }
    public PromoContentDto? Promo { get; set; }
    public GamificationContentDto? Gamification { get; set; }
    public ProgressiveFormContentDto? ProgressiveForm { get; set; }
    public GateContentDto? Gate { get; set; }
}

public class PromoContentDto
{
    public string Image { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string ButtonText { get; set; } = default!;
}

public class GamificationContentDto
{
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public bool ShowProgressBar { get; set; }
}

public class ProgressiveFormContentDto
{
    public List<FormFieldDto> Fields { get; set; } = new();
}

public class FormFieldDto
{
    public string FieldKey { get; set; } = default!;
    public string Label { get; set; } = default!;
    public FormFieldType Type { get; set; }
    public bool IsRequired { get; set; }
}

public class GateContentDto
{
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public List<FormFieldDto> RequiredFields { get; set; } = new();
}

public class CheckpointConfigDto
{
    public List<QuizCheckpointDto> Checkpoints { get; set; } = new List<QuizCheckpointDto>();
}
