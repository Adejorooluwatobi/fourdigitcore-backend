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

public class SuperAdminGovernanceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly AdminUserManagementService _adminService;

    public SuperAdminGovernanceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenMock = new Mock<IJwtTokenService>();
        var emailServiceMock = new Mock<IEmailService>();

        _adminService = new AdminUserManagementService(
            _context,
            passwordHasherMock.Object,
            jwtTokenMock.Object,
            emailServiceMock.Object);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task SuspendAdminAsync_ShouldDeactivateUserAndRevokeAllSessions()
    {
        // Arrange
        var user = new AdminUser
        {
            Email = "operator@6174.io",
            FullName = "Cluster Operator",
            IsActive = true,
            IsSuperAdmin = false
        };
        await _context.AdminUsers.AddAsync(user);

        var session1 = new Session { UserId = user.Id, RefreshTokenHash = "hash1", ExpiresAt = DateTime.UtcNow.AddDays(7) };
        var session2 = new Session { UserId = user.Id, RefreshTokenHash = "hash2", ExpiresAt = DateTime.UtcNow.AddDays(7) };
        await _context.Sessions.AddRangeAsync(session1, session2);
        await _context.SaveChangesAsync();

        // Act
        var success = await _adminService.SuspendAdminAsync(user.Id, Guid.NewGuid());

        // Assert
        success.Should().BeTrue();

        var updatedUser = await _context.AdminUsers.FindAsync(user.Id);
        updatedUser!.IsActive.Should().BeFalse();

        var activeSessions = await _context.Sessions.Where(s => s.UserId == user.Id && s.RevokedAt == null).ToListAsync();
        activeSessions.Should().BeEmpty("All active sessions must be terminated upon suspension");

        var audit = await _context.AuditLogs.FirstOrDefaultAsync(a => a.Action == "ADMIN_USER_SUSPENDED");
        audit.Should().NotBeNull();
        audit!.DiffPayload.Should().Contain("operator@6174.io");
    }

    [Fact]
    public async Task RootSuperAdmin_CannotBeSuspendedOrDeleted_InvariantGuaranteed()
    {
        // Arrange
        var rootAdmin = new AdminUser
        {
            Email = "Adejorotgold1@yahoo.com",
            FullName = "Oluwatobi Adejoro",
            IsActive = true,
            IsSuperAdmin = true
        };
        await _context.AdminUsers.AddAsync(rootAdmin);
        await _context.SaveChangesAsync();

        var callerId = Guid.NewGuid();

        // Act & Assert 1: Suspend attempt
        var actSuspend = async () => await _adminService.SuspendAdminAsync(rootAdmin.Id, callerId);
        await actSuspend.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Root System SuperAdmin account is invariant and cannot be suspended*");

        // Act & Assert 2: Delete attempt
        var actDelete = async () => await _adminService.DeleteAdminAsync(rootAdmin.Id, callerId);
        await actDelete.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Root System SuperAdmin account is invariant and cannot be deleted*");
    }

    [Fact]
    public async Task ReactivateAdminAsync_ShouldReEnableUser()
    {
        // Arrange
        var user = new AdminUser
        {
            Email = "suspended@6174.io",
            FullName = "Suspended Engineer",
            IsActive = false
        };
        await _context.AdminUsers.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var success = await _adminService.ReactivateAdminAsync(user.Id, Guid.NewGuid());

        // Assert
        success.Should().BeTrue();
        var updated = await _context.AdminUsers.FindAsync(user.Id);
        updated!.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAdminAsync_ShouldRemoveNonRootUserFromDatabase()
    {
        // Arrange
        var user = new AdminUser
        {
            Email = "temp@6174.io",
            FullName = "Temporary Contractor",
            IsActive = true,
            IsSuperAdmin = false
        };
        await _context.AdminUsers.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var success = await _adminService.DeleteAdminAsync(user.Id, Guid.NewGuid());

        // Assert
        success.Should().BeTrue();
        var exists = await _context.AdminUsers.AnyAsync(u => u.Id == user.Id);
        exists.Should().BeFalse();

        var audit = await _context.AuditLogs.FirstOrDefaultAsync(a => a.Action == "ADMIN_USER_DELETED");
        audit.Should().NotBeNull();
    }
}
