namespace QuizSvc.Share.Enums;
public enum QuizType
{
    Unknown = 0,
    Graded = 1,
    Dimension = 2
}

public enum QuizSource
{
    Unknown = 0,
    Template = 1,
    DeepCopy = 2,
    Refference = 3,
    TenantCreated = 4
}

public enum AttemptStatus
{
    Unknown = 0,
    InProgress = 1,
    Completed = 2
}
