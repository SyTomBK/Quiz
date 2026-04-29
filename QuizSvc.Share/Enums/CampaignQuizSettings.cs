namespace QuizSvc.Share.Enums;

public enum CheckpointTriggerType
{
    AtQuestionNumber = 1,
    EveryXQuestions = 2,
    AtProgressPercent = 3,
    BeforeResult = 4,
}
public enum CheckpointContentType
{
    Promo = 1,           // 3.1
    Gamification = 2,    // 3.2
    ProgressiveForm = 3, // 3.3
    Gate = 4             // 4.1
}

public enum LeadField
{
    FullName = 1,
    Email = 2,
    Phone = 3,
    Industry = 4,
    University = 5
}

public enum FormFieldType
{
    Text = 1,
    Email = 2,
    Phone = 3,
    Select = 4
}