namespace QuizSvc.Domain.Entities;
public class CheckpointConfig
{
    public List<QuizCheckpoint> Checkpoints { get; set; } = new List<QuizCheckpoint>();
}

public class QuizCheckpoint
{
    public string Key { get; set; } = default!;
    public CheckpointTrigger Trigger { get; set; } = default!;
    public CheckpointContent Content { get; set; } = default!;
}

public class CheckpointTrigger
{
    public CheckpointTriggerType CheckpointTriggerType { get; set; }
    public int? QuestionNumber { get; set; }
    public int? Interval { get; set; }
    public double? ProgressPercent { get; set; }
}

public class CheckpointContent
{
    public CheckpointContentType CheckpointContentType { get; set; }
    public PromoContent? Promo { get; set; }
    public GamificationContent? Gamification { get; set; }
    public ProgressiveFormContent? ProgressiveForm { get; set; }
    public GateContent? Gate { get; set; }
}

public class PromoContent
{
    public string Image { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string ButtonText { get; set; } = default!;
}

public class GamificationContent
{
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public bool ShowProgressBar { get; set; }
}
    
public class ProgressiveFormContent
{
    public List<FormField> Fields { get; set; } = new();
}

public class FormField
{
    public string FieldKey { get; set; } = default!;
    public string Label { get; set; } = default!;
    public FormFieldType Type { get; set; }
    public bool IsRequired { get; set; }
}

public class GateContent
{
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public List<FormField> RequiredFields { get; set; } = new();
}
