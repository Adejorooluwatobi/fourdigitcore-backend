using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Auth;
using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Application.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, string? ipAddress = null, string? userAgent = null, CancellationToken cancellationToken = default);
    Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request, string? ipAddress = null, string? userAgent = null, CancellationToken cancellationToken = default);
    Task<AdminUserDto?> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default);
    Task<bool> RevokeSessionAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default);
}

public class AuthService : IAuthService
{
    private readonly IAppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IAppDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request, string? ipAddress = null, string? userAgent = null, CancellationToken cancellationToken = default)
    {
        var user = await _context.AdminUsers
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.Trim().ToLower(), cancellationToken);

        if (user == null || !user.IsActive)
            return null;

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return null;

        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        var roles = user.UserRoles?.Where(ur => ur.Role != null).Select(ur => ur.Role!.Name).ToList() ?? new List<string>();
        var token = _jwtTokenService.GenerateToken(user, roles);
        var refreshToken = GenerateRefreshTokenString();

        var session = new Session
        {
            UserId = user.Id,
            RefreshTokenHash = HashToken(refreshToken),
            IpAddress = ipAddress,
            UserAgent = userAgent,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            RevokedAt = null
        };

        await _context.Sessions.AddAsync(session, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            Email = user.Email,
            FullName = user.FullName,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }

    public async Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request, string? ipAddress = null, string? userAgent = null, CancellationToken cancellationToken = default)
    {
        var hash = HashToken(request.RefreshToken);
        var session = await _context.Sessions
            .Include(s => s.User)
                .ThenInclude(u => u!.UserRoles)
                    .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(s => s.RefreshTokenHash == hash, cancellationToken);

        if (session == null || session.RevokedAt != null || session.ExpiresAt <= DateTime.UtcNow)
            return null;

        var user = session.User;
        if (user == null || !user.IsActive)
            return null;

        // Revoke old session and issue replacement
        session.RevokedAt = DateTime.UtcNow;
        session.UpdatedAt = DateTime.UtcNow;

        var roles = user.UserRoles?.Where(ur => ur.Role != null).Select(ur => ur.Role!.Name).ToList() ?? new List<string>();
        var newToken = _jwtTokenService.GenerateToken(user, roles);
        var newRefreshToken = GenerateRefreshTokenString();

        var newSession = new Session
        {
            UserId = user.Id,
            RefreshTokenHash = HashToken(newRefreshToken),
            IpAddress = ipAddress,
            UserAgent = userAgent,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            RevokedAt = null
        };

        await _context.Sessions.AddAsync(newSession, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            Email = user.Email,
            FullName = user.FullName,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }

    public async Task<AdminUserDto?> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.AdminUsers
            .AsNoTracking()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return null;

        return new AdminUserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            IsActive = user.IsActive,
            Roles = user.UserRoles?.Where(ur => ur.Role != null).Select(ur => ur.Role!.Name).ToList() ?? new List<string>(),
            LastLoginAt = user.LastLoginAt
        };
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _context.AdminUsers
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
            return false;

        if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            return false;

        user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> RevokeSessionAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default)
    {
        var hash = HashToken(refreshToken);
        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.RefreshTokenHash == hash, cancellationToken);

        if (session == null)
            return false;

        session.RevokedAt = DateTime.UtcNow;
        session.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static string HashToken(string token)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }
}
