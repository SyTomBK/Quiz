using System.ComponentModel;
using QuizSvc.Share.Enums;

namespace QuizSvc.Domain.Entities;
public class Lead : AuditableEntity<Guid>
{
    [Description("LeadId lấy từ FE")]
    public Guid LeadId { get; set; }
    public bool IsActive { get; set; }
    public Guid? LinkedUserId { get; set; }
    public User? LinkedUser { get; set; }
    public string? FullName { get; set; } 
    public string? PhoneNumber { get; set; } 
    public string? Email { get; set; } 
    public string? SchoolName { get; set; } 
    public string? Avatar { get; set; } 
    public string? ReferralCode { get; set; }
    public string? Tenant { get; set; }
    public Guid? CustomerId { get; set; }
    public LeadStatus Status { get; set; }
    public string? Note { get; set; }
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
