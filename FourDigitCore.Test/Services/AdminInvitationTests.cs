using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Application.Services;
using FourDigitCore.Domain.Entities;
using FourDigitCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FourDigitCore.Test.Services;

public class AdminInvitationTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtTokenService> _jwtTokenMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly AdminUserManagementService _adminService;

    public AdminInvitationTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _passwordHasherMock.Setup(h => h.HashPassword(It.IsAny<string>())).Returns((string p) => $"HASHED_{p}");
        _passwordHasherMock.Setup(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        _jwtTokenMock = new Mock<IJwtTokenService>();
        _jwtTokenMock.Setup(j => j.GenerateToken(It.IsAny<AdminUser>(), It.IsAny<IEnumerable<string>>())).Returns("mock.jwt.token");

        _emailServiceMock = new Mock<IEmailService>();
        _emailServiceMock.Setup(e => e.SendAdminInvitationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _adminService = new AdminUserManagementService(
            _context,
            _passwordHasherMock.Object,
            _jwtTokenMock.Object,
            _emailServiceMock.Object);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task InviteAdminAsync_ShouldCreateInvitationAndDispatchEmail()
    {
        // Arrange
        var currentAdminId = Guid.NewGuid();
        var dto = new InviteAdminDto
        {
            Email = "newarchitect@6174.io",
            FullName = "Elena Rostova",
            AssignedRole = "SYSTEMS_FELLOW"
        };

        // Act
        var result = await _adminService.InviteAdminAsync(dto, currentAdminId, "Root SuperAdmin");

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("newarchitect@6174.io");
        result.FullName.Should().Be("Elena Rostova");
        result.AssignedRole.Should().Be("SYSTEMS_FELLOW");
        result.IsAccepted.Should().BeFalse();
        result.ExpiresAt.Should().BeAfter(DateTime.UtcNow.AddDays(6));

        // Verify database persistence
        var savedInvite = await _context.AdminInvitations.FirstOrDefaultAsync(i => i.Email == "newarchitect@6174.io");
        savedInvite.Should().NotBeNull();
        savedInvite!.TokenHash.Should().HaveLength(64);

        // Verify email dispatched
        _emailServiceMock.Verify(e => e.SendAdminInvitationAsync(
            "newarchitect@6174.io",
            "Elena Rostova",
            "SYSTEMS_FELLOW",
            It.Is<string>(url => url.Contains("token=")),
            It.IsAny<CancellationToken>()), Times.Once);

        // Verify audit log
        var audit = await _context.AuditLogs.FirstOrDefaultAsync(a => a.Action == "ADMIN_INVITATION_SENT");
        audit.Should().NotBeNull();
        audit!.DiffPayload.Should().Contain("newarchitect@6174.io");
    }

    [Fact]
    public async Task InviteAdminAsync_WhenUserAlreadyExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var existingUser = new AdminUser
        {
            Email = "existing@6174.io",
            FullName = "Existing Admin",
            PasswordHash = "hash"
        };
        await _context.AdminUsers.AddAsync(existingUser);
        await _context.SaveChangesAsync();

        var dto = new InviteAdminDto
        {
            Email = "existing@6174.io",
            FullName = "Duplicate Admin",
            AssignedRole = "Admin"
        };

        // Act & Assert
        var act = async () => await _adminService.InviteAdminAsync(dto, Guid.NewGuid(), "Root");
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already exists*");
    }

    [Fact]
    public async Task AcceptInvitationAsync_WithValidToken_ShouldActivateUserAndReturnAuthTokens()
    {
        // Arrange: Generate raw token and store hashed invitation
        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

        var invitation = new AdminInvitation
        {
            Email = "cadet@6174.io",
            FullName = "Valentin Mercer",
            AssignedRole = "CONTENT_OPERATOR",
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsAccepted = false,
            IsRevoked = false
        };
        await _context.AdminInvitations.AddAsync(invitation);
        await _context.SaveChangesAsync();

        // Act 1: Check info
        var info = await _adminService.GetInvitationInfoAsync(rawToken);
        info.IsValid.Should().BeTrue();
        info.Email.Should().Be("cadet@6174.io");

        // Act 2: Accept invitation and set password
        var acceptDto = new AcceptInvitationRequestDto
        {
            Token = rawToken,
            Password = "SecurePassword123!",
            ConfirmPassword = "SecurePassword123!"
        };
        var loginResponse = await _adminService.AcceptInvitationAsync(acceptDto, "127.0.0.1");

        // Assert
        loginResponse.Should().NotBeNull();
        loginResponse.Email.Should().Be("cadet@6174.io");
        loginResponse.FullName.Should().Be("Valentin Mercer");
        loginResponse.Token.Should().Be("mock.jwt.token");
        loginResponse.RefreshToken.Should().NotBeNullOrWhiteSpace();

        // Verify user created in DB
        var user = await _context.AdminUsers.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(u => u.Email == "cadet@6174.io");
        user.Should().NotBeNull();
        user!.IsActive.Should().BeTrue();
        user.PasswordHash.Should().Be("HASHED_SecurePassword123!");
        user.UserRoles.Should().Contain(ur => ur.Role!.Name == "CONTENT_OPERATOR");

        // Verify invitation marked accepted
        var updatedInvite = await _context.AdminInvitations.FindAsync(invitation.Id);
        updatedInvite!.IsAccepted.Should().BeTrue();
        updatedInvite.AcceptedAt.Should().NotBeNull();

        // Verify single-use enforcement: Attempting to accept again should fail
        var reAccept = async () => await _adminService.AcceptInvitationAsync(acceptDto, "127.0.0.1");
        await reAccept.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetInvitationInfoAsync_WhenTokenIsExpired_ShouldReturnInvalid()
    {
        // Arrange
        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

        var expiredInvite = new AdminInvitation
        {
            Email = "expired@6174.io",
            FullName = "Expired Officer",
            AssignedRole = "ADMIN",
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(-10), // expired 10 minutes ago
            IsAccepted = false,
            IsRevoked = false
        };
        await _context.AdminInvitations.AddAsync(expiredInvite);
        await _context.SaveChangesAsync();

        // Act
        var info = await _adminService.GetInvitationInfoAsync(rawToken);

        // Assert
        info.IsValid.Should().BeFalse();
        info.ErrorMessage.Should().Contain("expired");
    }

    [Fact]
    public async Task GetInvitationInfoAsync_WhenTokenIsRevoked_ShouldReturnInvalid()
    {
        // Arrange
        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

        var revokedInvite = new AdminInvitation
        {
            Email = "revoked@6174.io",
            FullName = "Revoked Operator",
            AssignedRole = "ADMIN",
            TokenHash = tokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsAccepted = false,
            IsRevoked = true // explicitly revoked
        };
        await _context.AdminInvitations.AddAsync(revokedInvite);
        await _context.SaveChangesAsync();

        // Act
        var info = await _adminService.GetInvitationInfoAsync(rawToken);

        // Assert
        info.IsValid.Should().BeFalse();
        info.ErrorMessage.Should().Contain("revoked");
    }

    [Fact]
    public async Task AcceptInvitationAsync_WhenPasswordsDoNotMatch_ShouldThrowArgumentException()
    {
        // Arrange
        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var dto = new AcceptInvitationRequestDto
        {
            Token = rawToken,
            Password = "Password123!",
            ConfirmPassword = "DifferentPassword456!"
        };

        // Act & Assert
        var act = async () => await _adminService.AcceptInvitationAsync(dto, "127.0.0.1");
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Passwords do not match*");
    }

    [Fact]
    public async Task AcceptInvitationAsync_WhenPasswordTooShort_ShouldThrowArgumentException()
    {
        // Arrange
        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var dto = new AcceptInvitationRequestDto
        {
            Token = rawToken,
            Password = "short",
            ConfirmPassword = "short"
        };

        // Act & Assert
        var act = async () => await _adminService.AcceptInvitationAsync(dto, "127.0.0.1");
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*at least 8 characters*");
    }
}
