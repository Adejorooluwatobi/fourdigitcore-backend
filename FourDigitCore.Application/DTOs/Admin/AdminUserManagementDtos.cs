namespace FourDigitCore.Application.DTOs.Admin;

public class InviteAdminDto
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string AssignedRole { get; set; } = "Admin"; // "SUPERADMIN", "SYSTEMS_FELLOW", "CONTENT_OPERATOR", "Admin"
}

public class AdminInvitationDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string AssignedRole { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsAccepted { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    public string InvitedByAdminName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class InvitationInfoResponseDto
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string AssignedRole { get; set; } = string.Empty;
    public string InvitedByAdminName { get; set; } = string.Empty;
}

public class AcceptInvitationRequestDto
{
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class SystemAdminDetailDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public bool IsActive { get; set; }
    public bool IsSuperAdmin { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UploadMediaResultDto
{
    public bool Success { get; set; }
    public string Url { get; set; } = string.Empty;
    public string PublicId { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public long Bytes { get; set; }
    public string Message { get; set; } = string.Empty;
}
