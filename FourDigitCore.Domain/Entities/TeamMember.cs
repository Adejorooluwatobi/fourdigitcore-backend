using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class TeamMember : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string RoleTitle { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? PgpFingerprint { get; set; }
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
