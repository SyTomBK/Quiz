using QuizSvc.Application.Commands.Quizs;

namespace QuizSvc.Application.Services.Quizes;

public interface IQuizService
{
    Domain.Entities.Quiz DeepCopy (Domain.Entities.Quiz template, DeepCopyQuizCommand request);
    Task<Domain.Entities.Quiz> CreateQuiz(CreatedQuizCommand request, CancellationToken cancellationToken);
}
