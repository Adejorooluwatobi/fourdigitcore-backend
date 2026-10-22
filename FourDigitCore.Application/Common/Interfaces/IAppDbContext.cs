using Microsoft.EntityFrameworkCore;
using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Application.Common.Interfaces;

public interface IAppDbContext
{
    // Global & Layout
    DbSet<SiteSettings> SiteSettings { get; }
    DbSet<NavItem> NavItems { get; }
    DbSet<FooterColumn> FooterColumns { get; }
    DbSet<FooterLink> FooterLinks { get; }
    DbSet<PageSettings> PageSettings { get; }

    // Home Page & SDK
    DbSet<HomeHeroSection> HomeHeroSections { get; }
    DbSet<HomeHeroSlide> HomeHeroSlides { get; }
    DbSet<HomeQuoteSection> HomeQuoteSections { get; }
    DbSet<PlatformPillar> PlatformPillars { get; }
    DbSet<PlatformPillarSpec> PlatformPillarSpecs { get; }
    DbSet<PlatformPillarApplication> PlatformPillarApplications { get; }
    DbSet<HomeClosingCallout> HomeClosingCallouts { get; }
    DbSet<SdkShowcaseSection> SdkShowcaseSections { get; }
    DbSet<SdkCategory> SdkCategories { get; }
    DbSet<SdkCodeSnippet> SdkCodeSnippets { get; }

    // Solutions
    DbSet<SolutionsHeroSection> SolutionsHeroSections { get; }
    DbSet<SolutionDiscipline> SolutionDisciplines { get; }
    DbSet<SolutionDisciplineSpec> SolutionDisciplineSpecs { get; }
    DbSet<SolutionDisciplineApplication> SolutionDisciplineApplications { get; }

    // About & Leadership
    DbSet<AboutHeroSection> AboutHeroSections { get; }
    DbSet<AboutStorySection> AboutStorySections { get; }
    DbSet<ArchitectProfile> ArchitectProfiles { get; }
    DbSet<TeamMember> TeamMembers { get; }

    // Innovation, Whitepapers, Benchmarks
    DbSet<InnovationHeroSection> InnovationHeroSections { get; }
    DbSet<ConvergenceSection> ConvergenceSections { get; }
    DbSet<ConvergenceStep> ConvergenceSteps { get; }
    DbSet<Whitepaper> Whitepapers { get; }
    DbSet<WhitepaperDownload> WhitepaperDownloads { get; }
    DbSet<Patent> Patents { get; }
    DbSet<Benchmark> Benchmarks { get; }

    // Systems Academy
    DbSet<AcademyHeroSection> AcademyHeroSections { get; }
    DbSet<AcademyCurriculumSection> AcademyCurriculumSections { get; }
    DbSet<AcademyPhase> AcademyPhases { get; }
    DbSet<AcademyTrack> AcademyTracks { get; }
    DbSet<AcademyCohort> AcademyCohorts { get; }
    DbSet<StudentApplication> StudentApplications { get; }
    DbSet<StudentEnrollment> StudentEnrollments { get; }
    DbSet<Certification> Certifications { get; }

    // Careers & ATS
    DbSet<CareersHeroSection> CareersHeroSections { get; }
    DbSet<JobDepartment> JobDepartments { get; }
    DbSet<JobPosting> JobPostings { get; }
    DbSet<Candidate> Candidates { get; }
    DbSet<JobApplication> JobApplications { get; }
    DbSet<ApplicationNote> ApplicationNotes { get; }
    DbSet<ApplicationInterview> ApplicationInterviews { get; }

    // Launch Console & Inquiries
    DbSet<ContactPageSettings> ContactPageSettings { get; }
    DbSet<InquiryIntentOption> InquiryIntentOptions { get; }
    DbSet<InquiryScaleOption> InquiryScaleOptions { get; }
    DbSet<ContactInquiry> ContactInquiries { get; }
    DbSet<InquiryNote> InquiryNotes { get; }

    // Telemetry & Status
    DbSet<TelemetryNode> TelemetryNodes { get; }
    DbSet<TelemetrySnapshot> TelemetrySnapshots { get; }
    DbSet<MaintenanceAnnouncement> MaintenanceAnnouncements { get; }

    // Admin & Security
    DbSet<AdminUser> AdminUsers { get; }
    DbSet<Role> Roles { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<Session> Sessions { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<AdminInvitation> AdminInvitations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
