using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty; // "SUPERADMIN", "SALES_ADMIN", "RECRUITER", "ACADEMY_ADMIN", "CONTENT_ADMIN", "TELEMETRY_ADMIN"
    public string? Description { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class Permission : BaseEntity
{
    public string Code { get; set; } = string.Empty; // e.g. "inquiries:read", "jobs:write"
    public string? Description { get; set; }
    public string Module { get; set; } = string.Empty; // "INQUIRIES", "CAREERS", "ACADEMY", "CMS", "SECURITY"
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class UserRole
{
    public Guid UserId { get; set; }
    public AdminUser? User { get; set; }
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
}

public class RolePermission
{
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }
    public Guid PermissionId { get; set; }
    public Permission? Permission { get; set; }
}
