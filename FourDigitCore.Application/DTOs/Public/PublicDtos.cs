namespace FourDigitCore.Application.DTOs.Public;

// 1. Site Settings & Navigation
public class SiteSettingsDto
{
    public string SiteTitle { get; set; } = string.Empty;
    public string Monogram { get; set; } = string.Empty;
    public string LogoLightUrl { get; set; } = string.Empty;
    public string LogoDarkUrl { get; set; } = string.Empty;
    public string FaviconUrl { get; set; } = string.Empty;
    public string DefaultTheme { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string CopyrightText { get; set; } = string.Empty;
    public string FooterTagline { get; set; } = string.Empty;
    public string KernelStatusBadge { get; set; } = string.Empty;
    public string LatencyTickerText { get; set; } = string.Empty;
    public string ActiveClusterText { get; set; } = string.Empty;
    public string InvarianceObservedText { get; set; } = string.Empty;
}

public class NavItemDto
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string DataPage { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class FooterLinkDto
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class FooterColumnDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public List<FooterLinkDto> Links { get; set; } = new();
}

public class NavigationDto
{
    public List<NavItemDto> HeaderNav { get; set; } = new();
    public List<FooterColumnDto> FooterColumns { get; set; } = new();
}

// 2. Home Page (Platform)
public class HomeHeroSectionDto
{
    public string BadgeText { get; set; } = string.Empty;
    public string Headline { get; set; } = string.Empty;
    public string Subheadline { get; set; } = string.Empty;
    public string PrimaryCtaText { get; set; } = string.Empty;
    public string PrimaryCtaUrl { get; set; } = string.Empty;
    public string SecondaryCtaText { get; set; } = string.Empty;
    public string SecondaryCtaUrl { get; set; } = string.Empty;
    public string LatencyCeilingValue { get; set; } = string.Empty;
    public string LatencyCeilingLabel { get; set; } = string.Empty;
    public string StateModelValue { get; set; } = string.Empty;
    public string StateModelLabel { get; set; } = string.Empty;
    public string EnclaveSecurityValue { get; set; } = string.Empty;
    public string EnclaveSecurityLabel { get; set; } = string.Empty;
}

public class HomeHeroSlideDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class HomeQuoteSectionDto
{
    public string QuoteText { get; set; } = string.Empty;
    public string Attribution { get; set; } = string.Empty;
    public string InvariantTerm { get; set; } = string.Empty;
}

public class PlatformPillarSpecDto
{
    public string SpecText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class PlatformPillarAppDto
{
    public string ApplicationName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class PlatformPillarDto
{
    public Guid Id { get; set; }
    public string LayerNumber { get; set; } = string.Empty;
    public string BadgeText { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public List<PlatformPillarSpecDto> Specs { get; set; } = new();
    public List<PlatformPillarAppDto> Applications { get; set; } = new();
}

public class HomeClosingCalloutDto
{
    public string BadgeText { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PrimaryCtaText { get; set; } = string.Empty;
    public string PrimaryCtaUrl { get; set; } = string.Empty;
    public string SecondaryCtaText { get; set; } = string.Empty;
    public string SecondaryCtaUrl { get; set; } = string.Empty;
}

public class HomePlatformDto
{
    public HomeHeroSectionDto? Hero { get; set; }
    public List<HomeHeroSlideDto> Slides { get; set; } = new();
    public HomeQuoteSectionDto? Quote { get; set; }
    public List<PlatformPillarDto> Pillars { get; set; } = new();
    public HomeClosingCalloutDto? ClosingCallout { get; set; }
}

// 3. Dynamic Unbounded SDK Showcase
public class SdkCategoryDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class SdkSnippetDto
{
    public Guid Id { get; set; }
    public string LanguageKey { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string BindingTag { get; set; } = string.Empty;
    public string PackageTag { get; set; } = string.Empty;
    public string MemoryTag { get; set; } = string.Empty;
    public string ThreadsTag { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string CodeContent { get; set; } = string.Empty;
}

public class SdkShowcaseDto
{
    public string SectionKey { get; set; } = string.Empty;
    public string BadgeText { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SdkCategoryDto> Categories { get; set; } = new();
    public List<SdkSnippetDto> Snippets { get; set; } = new();
}

// 4. Solutions Page
public class SolutionSpecDto
{
    public string SpecText { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class SolutionAppDto
{
    public string ApplicationName { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class SolutionDisciplineDto
{
    public Guid Id { get; set; }
    public string DisciplineNumber { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CtaText { get; set; } = string.Empty;
    public string CtaUrl { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public List<SolutionSpecDto> Specs { get; set; } = new();
    public List<SolutionAppDto> Applications { get; set; } = new();
}

public class SolutionsDto
{
    public string BadgeText { get; set; } = string.Empty;
    public string Headline { get; set; } = string.Empty;
    public string Subheadline { get; set; } = string.Empty;
    public List<SolutionDisciplineDto> Disciplines { get; set; } = new();
}

// 5. About Page
public class TeamMemberDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string RoleTitle { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string PgpFingerprint { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public int SortOrder { get; set; }
}

public class ArchitectProfileDto
{
    public string Initials { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string RoleTitle { get; set; } = string.Empty;
    public string PgpKey { get; set; } = string.Empty;
    public string Quote { get; set; } = string.Empty;
    public string CommitsCountText { get; set; } = string.Empty;
    public string PatentsCountText { get; set; } = string.Empty;
    public string ReleasesCountText { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}

public class AboutDto
{
    public string BadgeText { get; set; } = string.Empty;
    public string Headline { get; set; } = string.Empty;
    public string Subheadline { get; set; } = string.Empty;
    public string HeroImageUrl { get; set; } = string.Empty;
    public string HeroHudPill { get; set; } = string.Empty;
    public string EquilibriumText { get; set; } = string.Empty;
    public string ClusterStateText { get; set; } = string.Empty;
    public string StoryBadgeText { get; set; } = string.Empty;
    public string StoryTitle { get; set; } = string.Empty;
    public string StoryParagraph1 { get; set; } = string.Empty;
    public string StoryParagraph2 { get; set; } = string.Empty;
    public ArchitectProfileDto? Architect { get; set; }
    public List<TeamMemberDto> TeamMembers { get; set; } = new();
}

// 6. Innovation Page
public class ConvergenceStepDto
{
    public string StepNumberText { get; set; } = string.Empty;
    public string FormulaText { get; set; } = string.Empty;
    public string? SubFormulaText { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsAttractor { get; set; }
    public int SortOrder { get; set; }
}

public class WhitepaperDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Authors { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Abstract { get; set; } = string.Empty;
    public string PdfUrl { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public int DownloadCount { get; set; }
    public DateTime PublishedAt { get; set; }
}

public class PatentDto
{
    public string PatentNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime FilingDate { get; set; }
    public DateTime? GrantDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Abstract { get; set; } = string.Empty;
    public string Inventors { get; set; } = string.Empty;
    public string? DocUrl { get; set; }
}

public class BenchmarkDto
{
    public string Subsystem { get; set; } = string.Empty;
    public string WorkloadType { get; set; } = string.Empty;
    public long ThroughputTps { get; set; }
    public double LatencyP50Ms { get; set; }
    public double LatencyP99Ms { get; set; }
    public int InvarianceCycles { get; set; }
    public string? ReproducibilityRepoUrl { get; set; }
}

public class InnovationDto
{
    public string BadgeText { get; set; } = string.Empty;
    public string Headline { get; set; } = string.Empty;
    public string Subheadline { get; set; } = string.Empty;
    public string HeroImageUrl { get; set; } = string.Empty;
    public string LabStatusPill { get; set; } = string.Empty;
    public string DieFabText { get; set; } = string.Empty;
    public string OpticalWaveguideText { get; set; } = string.Empty;
    public string LaneThroughputText { get; set; } = string.Empty;
    public string AttractorStateText { get; set; } = string.Empty;
    public string JitterVarianceText { get; set; } = string.Empty;
    public string ConvergenceBadgeText { get; set; } = string.Empty;
    public string ConvergenceTitle { get; set; } = string.Empty;
    public string ConvergenceDescription { get; set; } = string.Empty;
    public List<ConvergenceStepDto> ConvergenceSteps { get; set; } = new();
    public List<WhitepaperDto> Whitepapers { get; set; } = new();
    public List<PatentDto> Patents { get; set; } = new();
    public List<BenchmarkDto> Benchmarks { get; set; } = new();
}

// 7. Systems Academy Page
public class AcademyPhaseDto
{
    public string PhaseNumber { get; set; } = string.Empty;
    public string PhaseBadge { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class AcademyTrackDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public int TotalPhases { get; set; }
    public bool IsActive { get; set; }
}

public class AcademyCohortDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TrackSlug { get; set; } = string.Empty;
    public string TrackName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ApplicationDeadline { get; set; }
    public int MaxSeats { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class AcademyDto
{
    public string BadgeText { get; set; } = string.Empty;
    public string Headline { get; set; } = string.Empty;
    public string Subheadline { get; set; } = string.Empty;
    public string HeroImageUrl { get; set; } = string.Empty;
    public string LabStatusPill { get; set; } = string.Empty;
    public string CurriculumBadgeText { get; set; } = string.Empty;
    public string CurriculumTitle { get; set; } = string.Empty;
    public string CurriculumDescription { get; set; } = string.Empty;
    public List<AcademyPhaseDto> CurriculumPhases { get; set; } = new();
    public List<AcademyTrackDto> Tracks { get; set; } = new();
    public List<AcademyCohortDto> Cohorts { get; set; } = new();
}

// 8. Careers Page
public class JobDepartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}

public class JobPostingDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string LocationType { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = string.Empty;
    public string ExperienceLevel { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string TechStackTags { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsOpen { get; set; }
}

public class CareersDto
{
    public string BadgeText { get; set; } = string.Empty;
    public string Headline { get; set; } = string.Empty;
    public string Subheadline { get; set; } = string.Empty;
    public string HeroImageUrl { get; set; } = string.Empty;
    public string StatusPill { get; set; } = string.Empty;
    public List<JobDepartmentDto> Departments { get; set; } = new();
    public List<JobPostingDto> Jobs { get; set; } = new();
}

// 9. Telemetry & Cluster Status
public class TelemetryNodeDto
{
    public Guid Id { get; set; }
    public string NodeIdentifier { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string EnclaveType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double LatencyMs { get; set; }
    public int TpsThroughput { get; set; }
    public double JitterVariancePct { get; set; }
    public string ConvergenceState { get; set; } = string.Empty;
}

public class MaintenanceAnnouncementDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
}

public class TelemetryStatusDto
{
    public long AggregateTps { get; set; }
    public double P99LatencyMs { get; set; }
    public int ActiveNodesCount { get; set; }
    public decimal HealthScore { get; set; }
    public DateTime Timestamp { get; set; }
    public List<TelemetryNodeDto> Nodes { get; set; } = new();
    public List<MaintenanceAnnouncementDto> ActiveAnnouncements { get; set; } = new();
}
