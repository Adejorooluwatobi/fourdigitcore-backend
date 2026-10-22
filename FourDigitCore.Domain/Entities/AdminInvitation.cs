using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class AdminInvitation : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string AssignedRole { get; set; } = "Admin"; // "SUPERADMIN", "SYSTEMS_FELLOW", "CONTENT_OPERATOR", "Admin"
    public string TokenHash { get; set; } = string.Empty; // SHA-256 hash of single-use invitation token
    public DateTime ExpiresAt { get; set; }
    public bool IsAccepted { get; set; } = false;
    public DateTime? AcceptedAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public Guid InvitedByAdminId { get; set; }
    public string InvitedByAdminName { get; set; } = string.Empty;
}
