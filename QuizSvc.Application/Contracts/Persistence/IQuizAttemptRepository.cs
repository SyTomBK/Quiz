using QuizSvc.Application.Commands.QuizAttempts;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.QuizAttempts;

namespace QuizSvc.Application.Contracts.Persistence;

public interface IQuizAttemptRepository
{
    Task<QuizAttemptMiniResponseDto> CreateQuizAttempt(CreateQuizAttemptCommand request, CancellationToken cancellationToken);
    Task<QuizAttemptMiniResponseDto> UpdateQuizAttempt(UpdateQuizAttemptCommand request, CancellationToken cancellationToken);
    Task<QuizAttemptResponseDto> GetQuizAttemptQuery(GetQuizAttemptQueryQuery request, CancellationToken cancellationToken);
    Task<QuizResultMiniResponseDto> SubmitQuizAttempt(SubmitQuizAttemptCommand request, CancellationToken cancellationToken);
}
