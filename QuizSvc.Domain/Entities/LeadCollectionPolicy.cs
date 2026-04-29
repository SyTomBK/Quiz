namespace QuizSvc.Domain.Entities;
public class LeadCollectionPolicy
{
    public List<LeadField> RequiredFields { get; set; } = new List<LeadField>();
}
