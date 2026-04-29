using QuizSvc.Application.Dtos;
using QuizSvc.Application.Queries.QuizResults;
using QuizSvc.Share.Utils;

namespace QuizSvc.Application.Contracts.Persistence;
public interface IQuizResultRepository
{
    Task<QuizResultResponseDto> GetQuizResultDetail(Guid id, CancellationToken cancellationToken);
    Task<PagedList<CampaignQuizResultResponseDto>> GetQuizResultList(GetQuizResultListQuery request, CancellationToken cancellationToken);
}
