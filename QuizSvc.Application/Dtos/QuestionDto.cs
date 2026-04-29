namespace QuizSvc.Application.Dtos;

public class CreatedQuestionRequestDto
{
    public required string Content { get; set; }
    public required int Order { get; set; }
    public QuizType Type { get; set; }
    public List<CreatedAnswerRequestDto> Answers { get; set; } = new List<CreatedAnswerRequestDto>();
}

public class UpdatedQuestionRequestDto
{
    public Guid? Id { get; set; }
    public required string Content { get; set; }
    public required int Order { get; set; }
    public required List<UpdatedAnswerRequestDto> Answers { get; set; } = new List<UpdatedAnswerRequestDto>();
}

public class QuestionMiniResponseDto
{
    public required Guid Id { get; set; }
    public required string Content { get; set; }
    public required int Order { get; set; }
    public QuizType Type { get; set; }
    public required List<AnswerMiniResponseDto> Answers { get; set; }
}

public class QuestionSuccessResponseDto
{
    public required Guid Id { get; set; }
    public required string Content { get; set; }
    public required int Order { get; set; }
    public QuizType Type { get; set; }
}
public class CreatedQuizQuestionRequestDto
{
    public required string Content { get; set; }
    public required Guid QuizId { get; set; }
    public List<CreatedQuizAnswerRequestDto> Answers { get; set; } = [];
}