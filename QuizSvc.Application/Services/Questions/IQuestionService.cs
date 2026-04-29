using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Services.Questions;

public interface IQuestionService
{
    List<Question> Build(List<CreatedQuestionRequestDto> questions, QuizType quizType, Dictionary<string, Guid> dimensionMap);
}
