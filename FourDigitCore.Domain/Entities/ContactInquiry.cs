using FourDigitCore.Domain.Common;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Domain.Entities;

public class ContactInquiry : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Organization { get; set; }
    public string ScaleCategory { get; set; } = string.Empty;
    public string Intent { get; set; } = string.Empty;
    public string ScopeConstraints { get; set; } = string.Empty;
    public InquiryStatus Status { get; set; } = InquiryStatus.New;
    public InquiryPriority Priority { get; set; } = InquiryPriority.Normal;
    public string? AssignedAdminName { get; set; }
    public string? ClientIpHash { get; set; }
    public ICollection<InquiryNote> Notes { get; set; } = new List<InquiryNote>();
}

public class InquiryNote : BaseEntity
{
    public Guid InquiryId { get; set; }
    public ContactInquiry? Inquiry { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
