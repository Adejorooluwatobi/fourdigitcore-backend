using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class Session : BaseEntity
{
    public Guid UserId { get; set; }
    public AdminUser? User { get; set; }
    public string RefreshTokenHash { get; set; } = string.Empty;
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
}

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public AdminUser? User { get; set; }
    public string Action { get; set; } = string.Empty;     // "CREATE", "UPDATE", "DELETE", "STAGE_CHANGE", "LOGIN"
    public string EntityType { get; set; } = string.Empty; // "INQUIRY", "JOB_APPLICATION", "SDK_SNIPPET", etc.
    public Guid? EntityId { get; set; }
    public string? DiffPayload { get; set; }               // JSON diff before/after
    public string? IpAddress { get; set; }
}
