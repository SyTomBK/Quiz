namespace QuizSvc.Application.Dtos;

public class RecommendationMiniResponseDto
{
    public Guid Id { get; set; }
    public string Tenant { get; set; } = default!;
    public RecommendationType Type { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Order { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}