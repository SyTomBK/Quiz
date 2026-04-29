using QuizSvc.Application.Commands.Quizs;
using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.Quizs;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Contracts.Persistence;

public interface IQuizRepository
{
    Task EnsureQuizExist(Guid quizId, CancellationToken cancellationToken);
    Task<List<Guid>> GetExistingIds(List<Guid> ids, CancellationToken cancellationToken);
    Task<PagedList<QuizMiniResponseDto>> GetQuizList(GetQuizListQuery request, CancellationToken cancellationToken);
    Task<QuizResponseDto> GetQuizDetail(Guid id, CancellationToken cancellationToken);
    Task<Domain.Entities.Quiz?> GetTemplateById(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Domain.Entities.Quiz quiz, CancellationToken cancellationToken);
    Task<Domain.Entities.Quiz?> GetTemplateFullGraph(Guid id, CancellationToken cancellationToken);
    Task<GetQuizInCampaignDetailResponseDto> GetQuizInCampaignDetail(Guid quizId, Guid campaignId, CancellationToken cancellationToken);
    Task<Domain.Entities.Quiz?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Domain.Entities.Quiz?> GetWithQuestionAsync(Guid id, CancellationToken cancellationToken);
}
