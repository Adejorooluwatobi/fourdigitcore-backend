using Microsoft.EntityFrameworkCore;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Global & Layout
    public DbSet<SiteSettings> SiteSettings => Set<SiteSettings>();
    public DbSet<NavItem> NavItems => Set<NavItem>();
    public DbSet<FooterColumn> FooterColumns => Set<FooterColumn>();
    public DbSet<FooterLink> FooterLinks => Set<FooterLink>();
    public DbSet<PageSettings> PageSettings => Set<PageSettings>();

    // Home Page & SDK
    public DbSet<HomeHeroSection> HomeHeroSections => Set<HomeHeroSection>();
    public DbSet<HomeHeroSlide> HomeHeroSlides => Set<HomeHeroSlide>();
    public DbSet<HomeQuoteSection> HomeQuoteSections => Set<HomeQuoteSection>();
    public DbSet<PlatformPillar> PlatformPillars => Set<PlatformPillar>();
    public DbSet<PlatformPillarSpec> PlatformPillarSpecs => Set<PlatformPillarSpec>();
    public DbSet<PlatformPillarApplication> PlatformPillarApplications => Set<PlatformPillarApplication>();
    public DbSet<HomeClosingCallout> HomeClosingCallouts => Set<HomeClosingCallout>();
    public DbSet<SdkShowcaseSection> SdkShowcaseSections => Set<SdkShowcaseSection>();
    public DbSet<SdkCategory> SdkCategories => Set<SdkCategory>();
    public DbSet<SdkCodeSnippet> SdkCodeSnippets => Set<SdkCodeSnippet>();

    // Solutions
    public DbSet<SolutionsHeroSection> SolutionsHeroSections => Set<SolutionsHeroSection>();
    public DbSet<SolutionDiscipline> SolutionDisciplines => Set<SolutionDiscipline>();
    public DbSet<SolutionDisciplineSpec> SolutionDisciplineSpecs => Set<SolutionDisciplineSpec>();
    public DbSet<SolutionDisciplineApplication> SolutionDisciplineApplications => Set<SolutionDisciplineApplication>();

    // About & Leadership
    public DbSet<AboutHeroSection> AboutHeroSections => Set<AboutHeroSection>();
    public DbSet<AboutStorySection> AboutStorySections => Set<AboutStorySection>();
    public DbSet<ArchitectProfile> ArchitectProfiles => Set<ArchitectProfile>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

    // Innovation, Whitepapers, Benchmarks
    public DbSet<InnovationHeroSection> InnovationHeroSections => Set<InnovationHeroSection>();
    public DbSet<ConvergenceSection> ConvergenceSections => Set<ConvergenceSection>();
    public DbSet<ConvergenceStep> ConvergenceSteps => Set<ConvergenceStep>();
    public DbSet<Whitepaper> Whitepapers => Set<Whitepaper>();
    public DbSet<WhitepaperDownload> WhitepaperDownloads => Set<WhitepaperDownload>();
    public DbSet<Patent> Patents => Set<Patent>();
    public DbSet<Benchmark> Benchmarks => Set<Benchmark>();

    // Systems Academy
    public DbSet<AcademyHeroSection> AcademyHeroSections => Set<AcademyHeroSection>();
    public DbSet<AcademyCurriculumSection> AcademyCurriculumSections => Set<AcademyCurriculumSection>();
    public DbSet<AcademyPhase> AcademyPhases => Set<AcademyPhase>();
    public DbSet<AcademyTrack> AcademyTracks => Set<AcademyTrack>();
    public DbSet<AcademyCohort> AcademyCohorts => Set<AcademyCohort>();
    public DbSet<StudentApplication> StudentApplications => Set<StudentApplication>();
    public DbSet<StudentEnrollment> StudentEnrollments => Set<StudentEnrollment>();
    public DbSet<Certification> Certifications => Set<Certification>();

    // Careers & ATS
    public DbSet<CareersHeroSection> CareersHeroSections => Set<CareersHeroSection>();
    public DbSet<JobDepartment> JobDepartments => Set<JobDepartment>();
    public DbSet<JobPosting> JobPostings => Set<JobPosting>();
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<ApplicationNote> ApplicationNotes => Set<ApplicationNote>();
    public DbSet<ApplicationInterview> ApplicationInterviews => Set<ApplicationInterview>();

    // Launch Console & Inquiries
    public DbSet<ContactPageSettings> ContactPageSettings => Set<ContactPageSettings>();
    public DbSet<InquiryIntentOption> InquiryIntentOptions => Set<InquiryIntentOption>();
    public DbSet<InquiryScaleOption> InquiryScaleOptions => Set<InquiryScaleOption>();
    public DbSet<ContactInquiry> ContactInquiries => Set<ContactInquiry>();
    public DbSet<InquiryNote> InquiryNotes => Set<InquiryNote>();

    // Telemetry & Status
    public DbSet<TelemetryNode> TelemetryNodes => Set<TelemetryNode>();
    public DbSet<TelemetrySnapshot> TelemetrySnapshots => Set<TelemetrySnapshot>();
    public DbSet<MaintenanceAnnouncement> MaintenanceAnnouncements => Set<MaintenanceAnnouncement>();

    // Admin & Security
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<AdminInvitation> AdminInvitations => Set<AdminInvitation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global Layout
        modelBuilder.Entity<SiteSettings>(b =>
        {
            b.ToTable("site_settings");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<NavItem>(b =>
        {
            b.ToTable("nav_items");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<FooterColumn>(b =>
        {
            b.ToTable("footer_columns");
            b.HasKey(x => x.Id);
            b.HasMany(x => x.Links).WithOne(x => x.Column).HasForeignKey(x => x.ColumnId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FooterLink>(b =>
        {
            b.ToTable("footer_links");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<PageSettings>(b =>
        {
            b.ToTable("page_settings");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.PageKey).IsUnique();
        });

        // Home Page & SDK
        modelBuilder.Entity<HomeHeroSection>(b =>
        {
            b.ToTable("home_hero_sections");
            b.HasKey(x => x.Id);
            b.HasMany(x => x.Slides).WithOne(x => x.HeroSection).HasForeignKey(x => x.HeroSectionId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<HomeHeroSlide>(b =>
        {
            b.ToTable("home_hero_slides");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<HomeQuoteSection>(b =>
        {
            b.ToTable("home_quote_sections");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<PlatformPillar>(b =>
        {
            b.ToTable("platform_pillars");
            b.HasKey(x => x.Id);
            b.HasMany(x => x.Specs).WithOne(x => x.Pillar).HasForeignKey(x => x.PillarId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Applications).WithOne(x => x.Pillar).HasForeignKey(x => x.PillarId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PlatformPillarSpec>(b =>
        {
            b.ToTable("platform_pillar_specs");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<PlatformPillarApplication>(b =>
        {
            b.ToTable("platform_pillar_applications");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<HomeClosingCallout>(b =>
        {
            b.ToTable("home_closing_callouts");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<SdkShowcaseSection>(b =>
        {
            b.ToTable("sdk_showcase_sections");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.SectionKey).IsUnique();
            b.HasMany(x => x.Snippets).WithOne(x => x.Section).HasForeignKey(x => x.SectionId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SdkCategory>(b =>
        {
            b.ToTable("sdk_categories");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Slug).IsUnique();
            b.HasMany(x => x.Snippets).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<SdkCodeSnippet>(b =>
        {
            b.ToTable("sdk_code_snippets");
            b.HasKey(x => x.Id);
        });

        // Solutions
        modelBuilder.Entity<SolutionsHeroSection>(b =>
        {
            b.ToTable("solutions_hero_sections");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<SolutionDiscipline>(b =>
        {
            b.ToTable("solution_disciplines");
            b.HasKey(x => x.Id);
            b.HasMany(x => x.Specs).WithOne(x => x.Discipline).HasForeignKey(x => x.DisciplineId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Applications).WithOne(x => x.Discipline).HasForeignKey(x => x.DisciplineId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SolutionDisciplineSpec>(b =>
        {
            b.ToTable("solution_discipline_specs");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<SolutionDisciplineApplication>(b =>
        {
            b.ToTable("solution_discipline_applications");
            b.HasKey(x => x.Id);
        });

        // About & Leadership
        modelBuilder.Entity<AboutHeroSection>(b =>
        {
            b.ToTable("about_hero_sections");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<AboutStorySection>(b =>
        {
            b.ToTable("about_story_sections");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<ArchitectProfile>(b =>
        {
            b.ToTable("architect_profiles");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<TeamMember>(b =>
        {
            b.ToTable("team_members");
            b.HasKey(x => x.Id);
        });

        // Innovation
        modelBuilder.Entity<InnovationHeroSection>(b =>
        {
            b.ToTable("innovation_hero_sections");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<ConvergenceSection>(b =>
        {
            b.ToTable("convergence_sections");
            b.HasKey(x => x.Id);
            b.HasMany(x => x.Steps).WithOne(x => x.Section).HasForeignKey(x => x.SectionId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ConvergenceStep>(b =>
        {
            b.ToTable("convergence_steps");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Whitepaper>(b =>
        {
            b.ToTable("whitepapers");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Slug).IsUnique();
            b.HasMany(x => x.Downloads).WithOne(x => x.Whitepaper).HasForeignKey(x => x.WhitepaperId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WhitepaperDownload>(b =>
        {
            b.ToTable("whitepaper_downloads");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Patent>(b =>
        {
            b.ToTable("patents");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.PatentNumber).IsUnique();
        });

        modelBuilder.Entity<Benchmark>(b =>
        {
            b.ToTable("benchmarks");
            b.HasKey(x => x.Id);
        });

        // Academy
        modelBuilder.Entity<AcademyHeroSection>(b =>
        {
            b.ToTable("academy_hero_sections");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<AcademyCurriculumSection>(b =>
        {
            b.ToTable("academy_curriculum_sections");
            b.HasKey(x => x.Id);
            b.HasMany(x => x.Phases).WithOne(x => x.CurriculumSection).HasForeignKey(x => x.CurriculumSectionId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AcademyPhase>(b =>
        {
            b.ToTable("academy_phases");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<AcademyTrack>(b =>
        {
            b.ToTable("academy_tracks");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Slug).IsUnique();
            b.HasMany(x => x.Cohorts).WithOne(x => x.Track).HasForeignKey(x => x.TrackId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AcademyCohort>(b =>
        {
            b.ToTable("academy_cohorts");
            b.HasKey(x => x.Id);
            b.HasMany(x => x.Applications).WithOne(x => x.Cohort).HasForeignKey(x => x.CohortId).OnDelete(DeleteBehavior.Restrict);
            b.HasMany(x => x.Enrollments).WithOne(x => x.Cohort).HasForeignKey(x => x.CohortId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StudentApplication>(b =>
        {
            b.ToTable("student_applications");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<StudentEnrollment>(b =>
        {
            b.ToTable("student_enrollments");
            b.HasKey(x => x.Id);
            b.HasOne(x => x.Certification).WithOne(x => x.Enrollment).HasForeignKey<Certification>(x => x.EnrollmentId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Certification>(b =>
        {
            b.ToTable("certifications");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.CertCode).IsUnique();
            b.HasIndex(x => x.Sha256Hash).IsUnique();
        });

        // Careers & ATS
        modelBuilder.Entity<CareersHeroSection>(b =>
        {
            b.ToTable("careers_hero_sections");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<JobDepartment>(b =>
        {
            b.ToTable("job_departments");
            b.HasKey(x => x.Id);
            b.HasMany(x => x.JobPostings).WithOne(x => x.Department).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<JobPosting>(b =>
        {
            b.ToTable("job_postings");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Slug).IsUnique();
            b.HasMany(x => x.Applications).WithOne(x => x.Job).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Candidate>(b =>
        {
            b.ToTable("candidates");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Email).IsUnique();
            b.HasMany(x => x.Applications).WithOne(x => x.Candidate).HasForeignKey(x => x.CandidateId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<JobApplication>(b =>
        {
            b.ToTable("job_applications");
            b.HasKey(x => x.Id);
            b.HasMany(x => x.Notes).WithOne(x => x.Application).HasForeignKey(x => x.ApplicationId).OnDelete(DeleteBehavior.Cascade);
            b.HasMany(x => x.Interviews).WithOne(x => x.Application).HasForeignKey(x => x.ApplicationId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ApplicationNote>(b =>
        {
            b.ToTable("application_notes");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<ApplicationInterview>(b =>
        {
            b.ToTable("application_interviews");
            b.HasKey(x => x.Id);
        });

        // Launch Console
        modelBuilder.Entity<ContactPageSettings>(b =>
        {
            b.ToTable("contact_page_settings");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<InquiryIntentOption>(b =>
        {
            b.ToTable("inquiry_intent_options");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.IntentKey).IsUnique();
        });

        modelBuilder.Entity<InquiryScaleOption>(b =>
        {
            b.ToTable("inquiry_scale_options");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.ScaleKey).IsUnique();
        });

        modelBuilder.Entity<ContactInquiry>(b =>
        {
            b.ToTable("contact_inquiries");
            b.HasKey(x => x.Id);
            b.HasMany(x => x.Notes).WithOne(x => x.Inquiry).HasForeignKey(x => x.InquiryId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InquiryNote>(b =>
        {
            b.ToTable("inquiry_notes");
            b.HasKey(x => x.Id);
        });

        // Telemetry
        modelBuilder.Entity<TelemetryNode>(b =>
        {
            b.ToTable("telemetry_nodes");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.NodeIdentifier).IsUnique();
            b.HasMany(x => x.Snapshots).WithOne(x => x.Node).HasForeignKey(x => x.NodeId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TelemetrySnapshot>(b =>
        {
            b.ToTable("telemetry_snapshots");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.RecordedAt);
        });

        modelBuilder.Entity<MaintenanceAnnouncement>(b =>
        {
            b.ToTable("maintenance_announcements");
            b.HasKey(x => x.Id);
        });

        // Security & RBAC
        modelBuilder.Entity<AdminUser>(b =>
        {
            b.ToTable("admin_users");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Email).IsUnique();
            b.HasMany(x => x.Sessions).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Role>(b =>
        {
            b.ToTable("roles");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Permission>(b =>
        {
            b.ToTable("permissions");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<UserRole>(b =>
        {
            b.ToTable("user_roles");
            b.HasKey(x => new { x.UserId, x.RoleId });
            b.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RolePermission>(b =>
        {
            b.ToTable("role_permissions");
            b.HasKey(x => new { x.RoleId, x.PermissionId });
            b.HasOne(x => x.Role).WithMany(x => x.RolePermissions).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.Permission).WithMany(x => x.RolePermissions).HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Session>(b =>
        {
            b.ToTable("sessions");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<AuditLog>(b =>
        {
            b.ToTable("audit_logs");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.CreatedAt);
        });

        modelBuilder.Entity<AdminInvitation>(b =>
        {
            b.ToTable("admin_invitations");
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Email);
            b.HasIndex(x => x.TokenHash).IsUnique();
        });
    }
}
