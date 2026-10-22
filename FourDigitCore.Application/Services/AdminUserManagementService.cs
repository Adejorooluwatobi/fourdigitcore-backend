using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Application.DTOs.Auth;
using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Application.Services;

public class AdminUserManagementService : IAdminUserManagementService
{
    private readonly IAppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IEmailService _emailService;

    public AdminUserManagementService(
        IAppDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IEmailService emailService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _emailService = emailService;
    }

    // 1. List All Admins
    public async Task<List<SystemAdminDetailDto>> GetAllAdminsAsync(CancellationToken cancellationToken = default)
    {
        var users = await _context.AdminUsers
            .AsNoTracking()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .OrderByDescending(u => u.IsSuperAdmin)
            .ThenBy(u => u.CreatedAt)
            .ToListAsync(cancellationToken);

        return users.Select(u => new SystemAdminDetailDto
        {
            Id = u.Id,
            Email = u.Email,
            FullName = u.FullName,
            AvatarUrl = u.AvatarUrl,
            IsActive = u.IsActive,
            IsSuperAdmin = u.IsSuperAdmin,
            Roles = u.UserRoles.Select(ur => ur.Role?.Name ?? "Admin").ToList(),
            LastLoginAt = u.LastLoginAt,
            CreatedAt = u.CreatedAt
        }).ToList();
    }

    // 2. Invite Admin
    public async Task<AdminInvitationDto> InviteAdminAsync(InviteAdminDto dto, Guid currentAdminId, string currentAdminName, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = dto.Email.Trim().ToLowerInvariant();

        // Check if user already exists
        var existingUser = await _context.AdminUsers.AnyAsync(u => u.Email.ToLower() == normalizedEmail, cancellationToken);
        if (existingUser)
        {
            throw new InvalidOperationException("An administrative user with this email address already exists.");
        }

        // Revoke any previous pending invitations for this email
        var pendingInvites = await _context.AdminInvitations
            .Where(i => i.Email.ToLower() == normalizedEmail && !i.IsAccepted && !i.IsRevoked)
            .ToListAsync(cancellationToken);

        foreach (var invite in pendingInvites)
        {
            invite.IsRevoked = true;
            invite.UpdatedAt = DateTime.UtcNow;
        }

        // Generate 32-byte cryptographic token
        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

        var invitation = new AdminInvitation
        {
            Email = normalizedEmail,
            FullName = dto.FullName.Trim(),
            AssignedRole = string.IsNullOrWhiteSpace(dto.AssignedRole) ? "Admin" : dto.AssignedRole.Trim(),
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsAccepted = false,
            IsRevoked = false,
            InvitedByAdminId = currentAdminId,
            InvitedByAdminName = currentAdminName
        };

        await _context.AdminInvitations.AddAsync(invitation, cancellationToken);

        // Record forensic audit log
        var audit = new AuditLog
        {
            UserId = currentAdminId,
            Action = "ADMIN_INVITATION_SENT",
            EntityType = "ADMIN_INVITATION",
            EntityId = invitation.Id,
            DiffPayload = $"{{\"email\": \"{normalizedEmail}\", \"role\": \"{invitation.AssignedRole}\"}}"
        };
        await _context.AuditLogs.AddAsync(audit, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        // Dispatch Email
        var portalUrl = Environment.GetEnvironmentVariable("PORTAL_BASE_URL") ?? "http://localhost:5173";
        var inviteUrl = $"{portalUrl.TrimEnd('/')}/accept-invitation?token={rawToken}";

        await _emailService.SendAdminInvitationAsync(
            recipientEmail: invitation.Email,
            recipientName: invitation.FullName,
            role: invitation.AssignedRole,
            inviteUrl: inviteUrl,
            cancellationToken: cancellationToken);

        return new AdminInvitationDto
        {
            Id = invitation.Id,
            Email = invitation.Email,
            FullName = invitation.FullName,
            AssignedRole = invitation.AssignedRole,
            ExpiresAt = invitation.ExpiresAt,
            IsAccepted = invitation.IsAccepted,
            IsRevoked = invitation.IsRevoked,
            InvitedByAdminName = invitation.InvitedByAdminName,
            CreatedAt = invitation.CreatedAt
        };
    }

    // 3. View All Invitations
    public async Task<List<AdminInvitationDto>> GetInvitationsAsync(CancellationToken cancellationToken = default)
    {
        var invitations = await _context.AdminInvitations
            .AsNoTracking()
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync(cancellationToken);

        return invitations.Select(i => new AdminInvitationDto
        {
            Id = i.Id,
            Email = i.Email,
            FullName = i.FullName,
            AssignedRole = i.AssignedRole,
            ExpiresAt = i.ExpiresAt,
            IsAccepted = i.IsAccepted,
            AcceptedAt = i.AcceptedAt,
            IsRevoked = i.IsRevoked,
            InvitedByAdminName = i.InvitedByAdminName,
            CreatedAt = i.CreatedAt
        }).ToList();
    }

    // 4. Resend Invitation
    public async Task<bool> ResendInvitationAsync(Guid invitationId, CancellationToken cancellationToken = default)
    {
        var invitation = await _context.AdminInvitations.FirstOrDefaultAsync(i => i.Id == invitationId, cancellationToken);
        if (invitation == null) return false;
        if (invitation.IsAccepted) throw new InvalidOperationException("Cannot resend an invitation that has already been accepted.");

        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        invitation.TokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));
        invitation.ExpiresAt = DateTime.UtcNow.AddDays(7);
        invitation.IsRevoked = false;
        invitation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        var portalUrl = Environment.GetEnvironmentVariable("PORTAL_BASE_URL") ?? "http://localhost:5173";
        var inviteUrl = $"{portalUrl.TrimEnd('/')}/accept-invitation?token={rawToken}";

        await _emailService.SendAdminInvitationAsync(
            recipientEmail: invitation.Email,
            recipientName: invitation.FullName,
            role: invitation.AssignedRole,
            inviteUrl: inviteUrl,
            cancellationToken: cancellationToken);

        return true;
    }

    // 5. Revoke Invitation
    public async Task<bool> RevokeInvitationAsync(Guid invitationId, CancellationToken cancellationToken = default)
    {
        var invitation = await _context.AdminInvitations.FirstOrDefaultAsync(i => i.Id == invitationId, cancellationToken);
        if (invitation == null) return false;

        invitation.IsRevoked = true;
        invitation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // 6. Suspend Admin
    public async Task<bool> SuspendAdminAsync(Guid adminId, Guid currentAdminId, CancellationToken cancellationToken = default)
    {
        var user = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Id == adminId, cancellationToken);
        if (user == null) return false;

        // Root SuperAdmin Guard
        var rootEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "Adejorotgold1@yahoo.com";
        if (user.IsSuperAdmin || user.Email.Equals(rootEmail, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The Root System SuperAdmin account is invariant and cannot be suspended.");
        }

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;

        // Force-terminate all active sessions
        var activeSessions = await _context.Sessions
            .Where(s => s.UserId == adminId && s.RevokedAt == null)
            .ToListAsync(cancellationToken);

        foreach (var session in activeSessions)
        {
            session.RevokedAt = DateTime.UtcNow;
            session.UpdatedAt = DateTime.UtcNow;
        }

        var audit = new AuditLog
        {
            UserId = currentAdminId,
            Action = "ADMIN_USER_SUSPENDED",
            EntityType = "ADMIN_USER",
            EntityId = adminId,
            DiffPayload = $"{{\"email\": \"{user.Email}\", \"sessionsRevoked\": {activeSessions.Count}}}"
        };
        await _context.AuditLogs.AddAsync(audit, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // 7. Reactivate Admin
    public async Task<bool> ReactivateAdminAsync(Guid adminId, Guid currentAdminId, CancellationToken cancellationToken = default)
    {
        var user = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Id == adminId, cancellationToken);
        if (user == null) return false;

        user.IsActive = true;
        user.UpdatedAt = DateTime.UtcNow;

        var audit = new AuditLog
        {
            UserId = currentAdminId,
            Action = "ADMIN_USER_REACTIVATED",
            EntityType = "ADMIN_USER",
            EntityId = adminId,
            DiffPayload = $"{{\"email\": \"{user.Email}\"}}"
        };
        await _context.AuditLogs.AddAsync(audit, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // 8. Delete Admin
    public async Task<bool> DeleteAdminAsync(Guid adminId, Guid currentAdminId, CancellationToken cancellationToken = default)
    {
        var user = await _context.AdminUsers
            .Include(u => u.UserRoles)
            .Include(u => u.Sessions)
            .FirstOrDefaultAsync(u => u.Id == adminId, cancellationToken);

        if (user == null) return false;

        // Root SuperAdmin Guard
        var rootEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "Adejorotgold1@yahoo.com";
        if (user.IsSuperAdmin || user.Email.Equals(rootEmail, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The Root System SuperAdmin account is invariant and cannot be deleted.");
        }

        _context.Sessions.RemoveRange(user.Sessions);
        _context.UserRoles.RemoveRange(user.UserRoles);
        _context.AdminUsers.Remove(user);

        var audit = new AuditLog
        {
            UserId = currentAdminId,
            Action = "ADMIN_USER_DELETED",
            EntityType = "ADMIN_USER",
            EntityId = adminId,
            DiffPayload = $"{{\"deletedEmail\": \"{user.Email}\"}}"
        };
        await _context.AuditLogs.AddAsync(audit, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // 9. Public Onboarding: Query Invitation Info
    public async Task<InvitationInfoResponseDto> GetInvitationInfoAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return new InvitationInfoResponseDto { IsValid = false, ErrorMessage = "Token cannot be empty." };
        }

        var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token.Trim())));
        var invitation = await _context.AdminInvitations
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.TokenHash == tokenHash, cancellationToken);

        if (invitation == null)
        {
            return new InvitationInfoResponseDto { IsValid = false, ErrorMessage = "No invitation matches this security token." };
        }

        if (invitation.IsAccepted)
        {
            return new InvitationInfoResponseDto { IsValid = false, ErrorMessage = "This invitation has already been accepted." };
        }

        if (invitation.IsRevoked)
        {
            return new InvitationInfoResponseDto { IsValid = false, ErrorMessage = "This invitation has been revoked by the administrator." };
        }

        if (DateTime.UtcNow > invitation.ExpiresAt)
        {
            return new InvitationInfoResponseDto { IsValid = false, ErrorMessage = "This invitation has expired. Request a new invitation." };
        }

        return new InvitationInfoResponseDto
        {
            IsValid = true,
            Email = invitation.Email,
            FullName = invitation.FullName,
            AssignedRole = invitation.AssignedRole,
            InvitedByAdminName = invitation.InvitedByAdminName
        };
    }

    // 10. Public Onboarding: Accept Invitation & Set Password
    public async Task<LoginResponseDto> AcceptInvitationAsync(AcceptInvitationRequestDto dto, string? clientIp = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 8)
        {
            throw new ArgumentException("Password must be at least 8 characters long.");
        }

        if (dto.Password != dto.ConfirmPassword)
        {
            throw new ArgumentException("Passwords do not match.");
        }

        var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(dto.Token.Trim())));
        var invitation = await _context.AdminInvitations
            .FirstOrDefaultAsync(i => i.TokenHash == tokenHash, cancellationToken);

        if (invitation == null || invitation.IsAccepted || invitation.IsRevoked || DateTime.UtcNow > invitation.ExpiresAt)
        {
            throw new InvalidOperationException("Invalid, expired, or previously redeemed invitation token.");
        }

        // Verify user not already registered
        var existingUser = await _context.AdminUsers.AnyAsync(u => u.Email.ToLower() == invitation.Email.ToLower(), cancellationToken);
        if (existingUser)
        {
            throw new InvalidOperationException("An administrative account for this email already exists.");
        }

        // Create new Admin User
        var isSuperAdmin = invitation.AssignedRole.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase) ||
                           invitation.AssignedRole.Equals("SUPERADMIN", StringComparison.OrdinalIgnoreCase);

        var newUser = new AdminUser
        {
            Email = invitation.Email,
            FullName = invitation.FullName,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            IsActive = true,
            IsSuperAdmin = isSuperAdmin,
            LastLoginAt = DateTime.UtcNow
        };

        await _context.AdminUsers.AddAsync(newUser, cancellationToken);

        // Find or create role
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToUpper() == invitation.AssignedRole.ToUpper(), cancellationToken);
        if (role == null)
        {
            role = new Role { Name = invitation.AssignedRole.ToUpperInvariant(), Description = "Assigned via invitation" };
            await _context.Roles.AddAsync(role, cancellationToken);
        }

        await _context.UserRoles.AddAsync(new UserRole
        {
            User = newUser,
            Role = role
        }, cancellationToken);

        // Mark invitation as accepted
        invitation.IsAccepted = true;
        invitation.AcceptedAt = DateTime.UtcNow;
        invitation.UpdatedAt = DateTime.UtcNow;

        // Generate JWT Tokens
        var rolesList = new List<string> { role.Name };
        var accessToken = _jwtTokenService.GenerateToken(newUser, rolesList);

        var rawRefreshToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var refreshDays = int.TryParse(Environment.GetEnvironmentVariable("JWT_REFRESH_EXPIRE_DAYS"), out var days) ? days : 7;

        var session = new Session
        {
            UserId = newUser.Id,
            RefreshTokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawRefreshToken))),
            IpAddress = clientIp,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshDays)
        };
        await _context.Sessions.AddAsync(session, cancellationToken);

        // Audit Trail
        var audit = new AuditLog
        {
            UserId = newUser.Id,
            Action = "ADMIN_INVITATION_ACCEPTED",
            EntityType = "ADMIN_USER",
            EntityId = newUser.Id,
            IpAddress = clientIp,
            DiffPayload = $"{{\"email\": \"{newUser.Email}\", \"role\": \"{role.Name}\"}}"
        };
        await _context.AuditLogs.AddAsync(audit, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            Token = accessToken,
            RefreshToken = rawRefreshToken,
            Email = newUser.Email,
            FullName = newUser.FullName,
            Roles = rolesList,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshDays)
        };
    }
}
