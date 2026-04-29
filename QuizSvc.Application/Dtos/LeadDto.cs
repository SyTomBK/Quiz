using QuizSvc.Share.Enums;

namespace QuizSvc.Application.Dtos;

public class LeadMiniResponseDto
{
    public required Guid Id { get; set; }
    public required Guid LeadId { get; set; }
    public required bool IsActive { get; set; }
    public Guid? LinkedUserId { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? SchoolName { get; set; }
    public string? Avatar { get; set; }
    public string? ReferralCode { get; set; }
    public string? Tenant { get; set; }
    public Guid? CustomerId { get; set; }
    public LeadStatus Status { get; set; }
    public string? StatusName { get; set; }
    public string? Note { get; set; }
    public double TotalScore { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class LeadResponseDto
{
    public required Guid Id { get; set; }
    public required Guid LeadId { get; set; }
    public required bool IsActive { get; set; }
    public string? LinkedUserId { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? SchoolName { get; set; }
    public string? Avatar { get; set; }
    public string? ReferralCode { get; set; }
    public string? Tenant { get; set; }
    public Guid? CustomerId { get; set; }
    public LeadStatus Status { get; set; }
    public string? StatusName { get; set; }
    public string? Note { get; set; }
    public double TotalScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}
