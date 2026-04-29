using QuizSvc.Application.Dtos;

namespace QuizSvc.Application.Services.Questions;

public class QuestionService : IQuestionService
{
    public List<Question> Build( List<CreatedQuestionRequestDto> questions, QuizType quizType, Dictionary<string, Guid> dimensionMap )
    {
        return questions.Select(q => new Question
        {
            Id = Guid.NewGuid(),
            Content = q.Content,
            Type = quizType,
            Order = q.Order,
            Answers = BuildAnswers(q.Answers, quizType, dimensionMap)
        }).ToList();
    }

    private List<Answer> BuildAnswers(
     List<CreatedAnswerRequestDto> answers, QuizType quizType, Dictionary<string, Guid> dimensionMap)
    {
        var result = new List<Answer>();

        foreach (var a in answers)
        {
            var answer = new Answer
            {
                Id = Guid.NewGuid(),
                Content = a.Content,
                IsCorrect = a.IsCorrect
            };

            answer.AnswerDimensionScores =
                quizType == QuizType.Dimension && a.DimensionScores != null
                ? a.DimensionScores
                    .Where(ds => dimensionMap.ContainsKey(ds.TempDimId))
                    .Select(ds => new AnswerDimensionScore
                    {
                        Id = Guid.NewGuid(),
                        Answer = answer, 
                        DimensionId = dimensionMap[ds.TempDimId],
                        Score = ds.Score
                    }).ToList()
                : [];

            result.Add(answer);
        }

        return result;
    }
}
