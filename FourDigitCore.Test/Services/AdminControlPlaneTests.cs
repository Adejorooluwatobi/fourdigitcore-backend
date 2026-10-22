using FluentAssertions;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Application.Services;
using FourDigitCore.Domain.Entities;
using FourDigitCore.Domain.Enums;
using FourDigitCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FourDigitCore.Test.Services;

public class AdminControlPlaneTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly FourDigitCoreAdminService _adminService;
    private readonly FourDigitCorePublicService _publicService;
    private readonly Mock<IFileStorageService> _fileStorageMock;
    private readonly Mock<IEmailService> _emailServiceMock;

    public AdminControlPlaneTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _adminService = new FourDigitCoreAdminService(_context);
        _fileStorageMock = new Mock<IFileStorageService>();
        _emailServiceMock = new Mock<IEmailService>();
        _publicService = new FourDigitCorePublicService(_context, _fileStorageMock.Object, _emailServiceMock.Object);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task CreateSdkCategoryAndSnippet_ShouldDynamicallyExtendLanguageRegistry()
    {
        // Act - 1. Create Category
        var category = await _adminService.CreateSdkCategoryAsync(new UpsertSdkCategoryDto
        {
            Slug = "zig-systems",
            Name = "Zig Low-Level Kernel",
            SortOrder = 10
        });

        // Act - 2. Create Code Snippet
        var snippet = await _adminService.CreateSdkSnippetAsync(new UpsertSdkSnippetDto
        {
            CategoryId = category.Id,
            LanguageKey = "zig",
            DisplayName = "Zig 0.13.0",
            CodeContent = "const std = @import(\"std\");\npub fn main() void { std.debug.print(\"6174\\n\", .{}); }",
            FileName = "convergence.zig",
            BindingTag = "NATIVE",
            PackageTag = "zig-6174-core",
            MemoryTag = "0.00MB LEAK",
            ThreadsTag = "LOCK-FREE",
            SortOrder = 1,
            IsActive = true
        });

        // Assert
        category.Should().NotBeNull();
        category.Id.Should().NotBeEmpty();

        snippet.Should().NotBeNull();
        snippet.CategoryId.Should().Be(category.Id);
        snippet.LanguageKey.Should().Be("zig");

        // Verify public query retrieves the dynamically added snippet
        var publicSdk = await _publicService.GetSdkShowcaseAsync();
        publicSdk.Categories.Should().Contain(c => c.Name == "Zig Low-Level Kernel");
        publicSdk.Snippets.Should().Contain(s => s.LanguageKey == "zig" && s.FileName == "convergence.zig");
    }

    [Fact]
    public async Task InquiryWorkflow_TriageAndAddNote_ShouldPersistAuditRecords()
    {
        // Arrange
        var inquiry = new ContactInquiry
        {
            FullName = "Elena Rostova",
            Email = "elena@quantum-ledger.fi",
            Organization = "Quantum Ledger Labs",
            ScaleCategory = "Tier-1 Enterprise Infrastructure",
            Intent = "Deployment Architecture Consultation",
            ScopeConstraints = "Deploying deterministic attractor nodes across 12 datacenters",
            Status = InquiryStatus.New,
            Priority = InquiryPriority.Normal
        };
        await _context.ContactInquiries.AddAsync(inquiry);
        await _context.SaveChangesAsync();

        // Act 1: Update status & assign
        var updated = await _adminService.UpdateInquiryStatusAsync(inquiry.Id, new UpdateInquiryStatusDto
        {
            Status = "InReview",
            Priority = "Urgent",
            AssignedAdminName = "Lead Systems Architect"
        });

        // Act 2: Add triage note
        var note = await _adminService.AddInquiryNoteAsync(inquiry.Id, new AddInquiryNoteDto
        {
            AuthorName = "Lead Systems Architect",
            Content = "Confirmed enterprise scale. Initial call scheduled for consensus topology review."
        });

        // Assert
        updated.Should().BeTrue();
        note.Should().NotBeNull();
        note!.Content.Should().Contain("Initial call scheduled");

        var refreshedInquiry = await _adminService.GetInquiryByIdAsync(inquiry.Id);
        refreshedInquiry.Should().NotBeNull();
        refreshedInquiry!.Status.Should().Be(InquiryStatus.InReview);
        refreshedInquiry.Priority.Should().Be(InquiryPriority.Urgent);
        refreshedInquiry.AssignedAdminName.Should().Be("Lead Systems Architect");
        refreshedInquiry.Notes.Should().HaveCount(1);
    }

    [Fact]
    public async Task IssueCertificateAndVerify_ShouldProduceValidSha256AttractorRecord()
    {
        // Arrange
        var track = new AcademyTrack
        {
            Name = "Distributed Consensus & Deterministic Fault-Tolerance",
            Slug = "distributed-consensus",
            Summary = "Comprehensive consensus engineering track",
            TotalPhases = 5,
            IsActive = true
        };
        await _context.AcademyTracks.AddAsync(track);

        var cohort = new AcademyCohort
        {
            Name = "Spring 2026 Alpha",
            TrackId = track.Id,
            Status = CohortStatus.Enrolling,
            MaxSeats = 30,
            ApplicationDeadline = DateTime.UtcNow.AddMonths(1),
            StartDate = DateTime.UtcNow.AddMonths(2),
            EndDate = DateTime.UtcNow.AddMonths(5)
        };
        await _context.AcademyCohorts.AddAsync(cohort);
        await _context.SaveChangesAsync();

        var enrollment = new StudentEnrollment
        {
            CohortId = cohort.Id,
            StudentName = "Marcus Vance",
            StudentEmail = "marcus.vance@mit.edu",
            CurrentPhaseNumber = 5,
            EnrolledAt = DateTime.UtcNow.AddMonths(-3),
            IsGraduated = false
        };
        await _context.StudentEnrollments.AddAsync(enrollment);
        await _context.SaveChangesAsync();

        // Act 1: Issue certificate
        var certDto = await _adminService.IssueCertificateAsync(new IssueCertificateDto
        {
            EnrollmentId = enrollment.Id,
            RecipientName = "Marcus Vance",
            TrackName = "Distributed Consensus & Deterministic Fault-Tolerance",
            SignedByAdminName = "Dr. D. R. Kaprekar Fellow"
        });

        // Assert Issue
        certDto.Should().NotBeNull();
        certDto.CertCode.Should().StartWith("6174-CERT-");
        certDto.Sha256Hash.Should().HaveLength(64, "SHA-256 produces a 64-character hexadecimal digest");
        certDto.RecipientName.Should().Be("Marcus Vance");

        // Act 2: Verify Certificate via Public Endpoint
        var verification = await _publicService.VerifyCertificateAsync(certDto.CertCode);

        // Assert Public Verification
        verification.Should().NotBeNull();
        verification.IsValid.Should().BeTrue();
        verification.VerificationCode.Should().Be(certDto.CertCode);
        verification.StudentName.Should().Be("Marcus Vance");
        verification.Sha256Hash.Should().Be(certDto.Sha256Hash);
        verification.VerificationMessage.Should().Contain("Cryptographic integrity verified");
    }

    [Fact]
    public async Task LogActionAsync_ShouldPersistAuditLogEntriesWithMetadata()
    {
        // Arrange
        var adminId = Guid.NewGuid();

        // Act
        await _adminService.LogActionAsync(
            userId: adminId,
            action: "CLUSTER_TOPOLOGY_UPDATED",
            entityType: "TelemetryNode",
            entityId: Guid.NewGuid(),
            diffJson: "{\"status\": \"Degraded\", \"newStatus\": \"Nominal\"}",
            clientIp: "10.0.0.1");

        // Assert
        var logs = await _adminService.GetAuditLogsAsync(page: 1, pageSize: 10);
        logs.Should().ContainSingle();
        logs[0].Action.Should().Be("CLUSTER_TOPOLOGY_UPDATED");
        logs[0].EntityType.Should().Be("TELEMETRYNODE");
        logs[0].IpAddress.Should().Be("10.0.0.1");
        logs[0].DiffPayload.Should().Contain("Degraded");
    }
}
