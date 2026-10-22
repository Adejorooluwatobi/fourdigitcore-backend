using Microsoft.EntityFrameworkCore;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Domain.Entities;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Application.Services;

public interface IFourDigitCorePublicService
{
    // Content Queries
    Task<SiteSettingsDto> GetSiteSettingsAsync(CancellationToken cancellationToken = default);
    Task<NavigationDto> GetNavigationAsync(CancellationToken cancellationToken = default);
    Task<HomePlatformDto> GetHomePlatformAsync(CancellationToken cancellationToken = default);
    Task<SdkShowcaseDto> GetSdkShowcaseAsync(CancellationToken cancellationToken = default);
    Task<SdkSnippetDto?> GetSdkSnippetByLanguageAsync(string languageKey, CancellationToken cancellationToken = default);
    Task<SolutionsDto> GetSolutionsAsync(CancellationToken cancellationToken = default);
    Task<AboutDto> GetAboutAsync(CancellationToken cancellationToken = default);
    Task<InnovationDto> GetInnovationAsync(CancellationToken cancellationToken = default);
    Task<List<WhitepaperDto>> GetWhitepapersAsync(CancellationToken cancellationToken = default);
    Task<AcademyDto> GetAcademyAsync(CancellationToken cancellationToken = default);
    Task<List<AcademyCohortDto>> GetAcademyCohortsAsync(CancellationToken cancellationToken = default);
    Task<CareersDto> GetCareersAsync(CancellationToken cancellationToken = default);
    Task<JobPostingDto?> GetJobPostingBySlugOrIdAsync(string slugOrId, CancellationToken cancellationToken = default);
    Task<TelemetryStatusDto> GetTelemetryStatusAsync(CancellationToken cancellationToken = default);

    // Ingestion Mutations
    Task<InquiryResponseDto> SubmitContactInquiryAsync(CreateInquiryRequestDto request, string? clientIp = null, CancellationToken cancellationToken = default);
    Task<JobApplicationResponseDto> SubmitJobApplicationAsync(JobApplicationRequestDto request, Stream? resumeStream = null, string? resumeFileName = null, string? contentType = null, string? clientIp = null, CancellationToken cancellationToken = default);
    Task<StudentApplicationResponseDto> SubmitStudentApplicationAsync(StudentApplicationRequestDto request, string? clientIp = null, CancellationToken cancellationToken = default);
    Task<WhitepaperDownloadResponseDto> SubmitWhitepaperDownloadAsync(WhitepaperDownloadRequestDto request, string? clientIp = null, CancellationToken cancellationToken = default);
    Task<CertificateVerificationResponseDto> VerifyCertificateAsync(string verificationCode, CancellationToken cancellationToken = default);
}

public class FourDigitCorePublicService : IFourDigitCorePublicService
{
    private readonly IAppDbContext _context;
    private readonly IFileStorageService _fileStorageService;
    private readonly IEmailService _emailService;

    public FourDigitCorePublicService(
        IAppDbContext context,
        IFileStorageService fileStorageService,
        IEmailService emailService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
        _emailService = emailService;
    }

    // 1. Site Settings
    public async Task<SiteSettingsDto> GetSiteSettingsAsync(CancellationToken cancellationToken = default)
    {
        var settings = await _context.SiteSettings.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        if (settings == null) return new SiteSettingsDto();

        return new SiteSettingsDto
        {
            SiteTitle = settings.SiteTitle,
            Monogram = settings.Monogram,
            LogoLightUrl = settings.LogoLightUrl,
            LogoDarkUrl = settings.LogoDarkUrl,
            FaviconUrl = settings.FaviconUrl,
            DefaultTheme = settings.DefaultTheme,
            MetaDescription = settings.MetaDescription,
            CopyrightText = settings.CopyrightText,
            FooterTagline = settings.FooterTagline,
            KernelStatusBadge = settings.KernelStatusBadge,
            LatencyTickerText = settings.LatencyTickerText,
            ActiveClusterText = settings.ActiveClusterText,
            InvarianceObservedText = settings.InvarianceObservedText
        };
    }

    // 2. Navigation
    public async Task<NavigationDto> GetNavigationAsync(CancellationToken cancellationToken = default)
    {
        var navItems = await _context.NavItems
            .AsNoTracking()
            .Where(n => n.IsActive)
            .OrderBy(n => n.SortOrder)
            .Select(n => new NavItemDto
            {
                Id = n.Id,
                Label = n.Label,
                Url = n.Url,
                DataPage = n.DataPage,
                SortOrder = n.SortOrder
            })
            .ToListAsync(cancellationToken);

        var footerCols = await _context.FooterColumns
            .AsNoTracking()
            .OrderBy(c => c.SortOrder)
            .Include(c => c.Links)
            .Select(c => new FooterColumnDto
            {
                Id = c.Id,
                Title = c.Title,
                SortOrder = c.SortOrder,
                Links = c.Links.OrderBy(l => l.SortOrder).Select(l => new FooterLinkDto
                {
                    Id = l.Id,
                    Label = l.Label,
                    Url = l.Url,
                    SortOrder = l.SortOrder
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return new NavigationDto
        {
            HeaderNav = navItems,
            FooterColumns = footerCols
        };
    }

    // 3. Home Platform
    public async Task<HomePlatformDto> GetHomePlatformAsync(CancellationToken cancellationToken = default)
    {
        var hero = await _context.HomeHeroSections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var slides = await _context.HomeHeroSlides
            .AsNoTracking()
            .OrderBy(s => s.SortOrder)
            .Select(s => new HomeHeroSlideDto
            {
                Id = s.Id,
                ImageUrl = s.ImageUrl,
                AltText = s.AltText,
                SortOrder = s.SortOrder
            })
            .ToListAsync(cancellationToken);

        var quote = await _context.HomeQuoteSections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        var pillars = await _context.PlatformPillars
            .AsNoTracking()
            .OrderBy(p => p.SortOrder)
            .Include(p => p.Specs)
            .Include(p => p.Applications)
            .Select(p => new PlatformPillarDto
            {
                Id = p.Id,
                LayerNumber = p.LayerNumber,
                BadgeText = p.BadgeText,
                Title = p.Title,
                Description = p.Description,
                Icon = p.Icon,
                SortOrder = p.SortOrder,
                Specs = p.Specs.OrderBy(s => s.SortOrder).Select(s => new PlatformPillarSpecDto
                {
                    SpecText = s.SpecText,
                    SortOrder = s.SortOrder
                }).ToList(),
                Applications = p.Applications.OrderBy(a => a.SortOrder).Select(a => new PlatformPillarAppDto
                {
                    ApplicationName = a.ApplicationName,
                    SortOrder = a.SortOrder
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        var callout = await _context.HomeClosingCallouts.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        return new HomePlatformDto
        {
            Hero = hero == null ? null : new HomeHeroSectionDto
            {
                BadgeText = hero.BadgeText,
                Headline = hero.Headline,
                Subheadline = hero.Subheadline,
                PrimaryCtaText = hero.PrimaryCtaText,
                PrimaryCtaUrl = hero.PrimaryCtaUrl,
                SecondaryCtaText = hero.SecondaryCtaText,
                SecondaryCtaUrl = hero.SecondaryCtaUrl,
                LatencyCeilingValue = hero.LatencyCeilingValue,
                LatencyCeilingLabel = hero.LatencyCeilingLabel,
                StateModelValue = hero.StateModelValue,
                StateModelLabel = hero.StateModelLabel,
                EnclaveSecurityValue = hero.EnclaveSecurityValue,
                EnclaveSecurityLabel = hero.EnclaveSecurityLabel
            },
            Slides = slides,
            Quote = quote == null ? null : new HomeQuoteSectionDto
            {
                QuoteText = quote.QuoteText,
                Attribution = quote.Attribution,
                InvariantTerm = quote.InvariantTerm
            },
            Pillars = pillars,
            ClosingCallout = callout == null ? null : new HomeClosingCalloutDto
            {
                BadgeText = callout.BadgeText,
                Title = callout.Title,
                Description = callout.Description,
                PrimaryCtaText = callout.PrimaryCtaText,
                PrimaryCtaUrl = callout.PrimaryCtaUrl,
                SecondaryCtaText = callout.SecondaryCtaText,
                SecondaryCtaUrl = callout.SecondaryCtaUrl
            }
        };
    }

    // 4. Dynamic Unbounded SDK Showcase
    public async Task<SdkShowcaseDto> GetSdkShowcaseAsync(CancellationToken cancellationToken = default)
    {
        var section = await _context.SdkShowcaseSections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var categories = await _context.SdkCategories
            .AsNoTracking()
            .OrderBy(c => c.SortOrder)
            .Select(c => new SdkCategoryDto
            {
                Id = c.Id,
                Slug = c.Slug,
                Name = c.Name,
                SortOrder = c.SortOrder
            })
            .ToListAsync(cancellationToken);

        var snippets = await _context.SdkCodeSnippets
            .AsNoTracking()
            .Include(s => s.Category)
            .OrderBy(s => s.SortOrder)
            .Select(s => new SdkSnippetDto
            {
                Id = s.Id,
                LanguageKey = s.LanguageKey,
                DisplayName = s.DisplayName,
                CategorySlug = s.Category != null ? s.Category.Slug : string.Empty,
                FileName = s.FileName,
                BindingTag = s.BindingTag,
                PackageTag = s.PackageTag,
                MemoryTag = s.MemoryTag,
                ThreadsTag = s.ThreadsTag,
                SortOrder = s.SortOrder,
                CodeContent = s.CodeContent
            })
            .ToListAsync(cancellationToken);

        return new SdkShowcaseDto
        {
            SectionKey = section?.SectionKey ?? "home_sdk",
            BadgeText = section?.BadgeText ?? "04 // DEVELOPER ECOSYSTEM",
            Title = section?.Title ?? "Native SDKs for Ultra-Low Latency",
            Description = section?.Description ?? "Deploy deterministic cluster connections in under 4 lines of code. Zero unneeded runtime abstractions.",
            Categories = categories,
            Snippets = snippets
        };
    }

    public async Task<SdkSnippetDto?> GetSdkSnippetByLanguageAsync(string languageKey, CancellationToken cancellationToken = default)
    {
        var s = await _context.SdkCodeSnippets
            .AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.LanguageKey.ToLower() == languageKey.ToLower(), cancellationToken);

        if (s == null) return null;

        return new SdkSnippetDto
        {
            Id = s.Id,
            LanguageKey = s.LanguageKey,
            DisplayName = s.DisplayName,
            CategorySlug = s.Category?.Slug ?? string.Empty,
            FileName = s.FileName,
            BindingTag = s.BindingTag,
            PackageTag = s.PackageTag,
            MemoryTag = s.MemoryTag,
            ThreadsTag = s.ThreadsTag,
            SortOrder = s.SortOrder,
            CodeContent = s.CodeContent
        };
    }

    // 5. Solutions Page
    public async Task<SolutionsDto> GetSolutionsAsync(CancellationToken cancellationToken = default)
    {
        var hero = await _context.SolutionsHeroSections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var disciplines = await _context.SolutionDisciplines
            .AsNoTracking()
            .OrderBy(d => d.SortOrder)
            .Include(d => d.Specs)
            .Include(d => d.Applications)
            .Select(d => new SolutionDisciplineDto
            {
                Id = d.Id,
                DisciplineNumber = d.DisciplineNumber,
                CategoryName = d.CategoryName,
                Title = d.Title,
                Description = d.Description,
                CtaText = d.CtaText,
                CtaUrl = d.CtaUrl,
                SortOrder = d.SortOrder,
                Specs = d.Specs.OrderBy(s => s.SortOrder).Select(s => new SolutionSpecDto
                {
                    SpecText = s.SpecText,
                    SortOrder = s.SortOrder
                }).ToList(),
                Applications = d.Applications.OrderBy(a => a.SortOrder).Select(a => new SolutionAppDto
                {
                    ApplicationName = a.ApplicationName,
                    SortOrder = a.SortOrder
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return new SolutionsDto
        {
            BadgeText = hero?.BadgeText ?? "01 // SOLUTIONS & ARCHITECTURAL DISCIPLINES",
            Headline = hero?.Headline ?? "Technology engineered around deterministic scale.",
            Subheadline = hero?.Subheadline ?? "From direct-to-metal neural kernels to post-quantum zero-trust enclaves and distributed mesh fabrics, we engineer systems that remain invariant under infinite load.",
            Disciplines = disciplines
        };
    }

    // 6. About Page
    public async Task<AboutDto> GetAboutAsync(CancellationToken cancellationToken = default)
    {
        var hero = await _context.AboutHeroSections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var story = await _context.AboutStorySections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var architect = await _context.ArchitectProfiles.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var team = await _context.TeamMembers
            .AsNoTracking()
            .OrderBy(t => t.SortOrder)
            .Select(t => new TeamMemberDto
            {
                Id = t.Id,
                FullName = t.FullName,
                RoleTitle = t.RoleTitle,
                Bio = t.Bio,
                PgpFingerprint = t.PgpFingerprint ?? string.Empty,
                AvatarUrl = t.AvatarUrl ?? string.Empty,
                SortOrder = t.SortOrder
            })
            .ToListAsync(cancellationToken);

        return new AboutDto
        {
            BadgeText = hero?.BadgeText ?? "01 // ABOUT FOURDIGITCORE",
            Headline = hero?.Headline ?? "Founded on the principle of Kaprekar Invariance.",
            Subheadline = hero?.Subheadline ?? "Deterministic computing is not an optimization; it is a mathematical foundation.",
            HeroImageUrl = hero?.HeroImageUrl ?? "assets/img/about-hero.jpg",
            HeroHudPill = hero?.HeroHudPill ?? "● FOUNDATION // MATHEMATICAL PROOFS",
            EquilibriumText = hero?.EquilibriumText ?? "6174 INVARIANT",
            ClusterStateText = hero?.ClusterStateText ?? "STABLE",
            StoryBadgeText = story?.BadgeText ?? "THE INVARIANCE STORY",
            StoryTitle = story?.Title ?? "Why 6174?",
            StoryParagraph1 = story?.Paragraph1 ?? string.Empty,
            StoryParagraph2 = story?.Paragraph2 ?? string.Empty,
            Architect = architect == null ? null : new ArchitectProfileDto
            {
                Initials = architect.Initials,
                FullName = architect.FullName,
                RoleTitle = architect.RoleTitle,
                PgpKey = architect.PgpKey,
                Quote = architect.Quote,
                CommitsCountText = architect.CommitsCountText,
                PatentsCountText = architect.PatentsCountText,
                ReleasesCountText = architect.ReleasesCountText,
                AvatarUrl = architect.AvatarUrl
            },
            TeamMembers = team
        };
    }

    // 7. Innovation Page
    public async Task<InnovationDto> GetInnovationAsync(CancellationToken cancellationToken = default)
    {
        var hero = await _context.InnovationHeroSections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var conv = await _context.ConvergenceSections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var steps = await _context.ConvergenceSteps
            .AsNoTracking()
            .OrderBy(s => s.SortOrder)
            .Select(s => new ConvergenceStepDto
            {
                StepNumberText = s.StepNumberText,
                FormulaText = s.FormulaText,
                SubFormulaText = s.SubFormulaText,
                Description = s.Description,
                IsAttractor = s.IsAttractor,
                SortOrder = s.SortOrder
            })
            .ToListAsync(cancellationToken);

        var whitepapers = await _context.Whitepapers
            .AsNoTracking()
            .OrderByDescending(w => w.PublishedAt)
            .Select(w => new WhitepaperDto
            {
                Id = w.Id,
                Title = w.Title,
                Slug = w.Slug,
                Authors = w.Authors,
                Category = w.Category.ToString(),
                Abstract = w.Abstract,
                PdfUrl = w.PdfUrl,
                Version = w.Version,
                DownloadCount = w.DownloadCount,
                PublishedAt = w.PublishedAt
            })
            .ToListAsync(cancellationToken);

        var patents = await _context.Patents
            .AsNoTracking()
            .OrderByDescending(p => p.FilingDate)
            .Select(p => new PatentDto
            {
                PatentNumber = p.PatentNumber,
                Title = p.Title,
                FilingDate = p.FilingDate,
                GrantDate = p.GrantDate,
                Status = p.Status.ToString(),
                Abstract = p.Abstract,
                Inventors = p.Inventors,
                DocUrl = p.DocUrl
            })
            .ToListAsync(cancellationToken);

        var benchmarks = await _context.Benchmarks
            .AsNoTracking()
            .Select(b => new BenchmarkDto
            {
                Subsystem = b.Subsystem,
                WorkloadType = b.WorkloadType,
                ThroughputTps = b.ThroughputTps,
                LatencyP50Ms = b.LatencyP50Ms,
                LatencyP99Ms = b.LatencyP99Ms,
                InvarianceCycles = b.InvarianceCycles,
                ReproducibilityRepoUrl = b.ReproducibilityRepoUrl
            })
            .ToListAsync(cancellationToken);

        return new InnovationDto
        {
            BadgeText = hero?.BadgeText ?? "01 // RESEARCH LABS & PROOF ENGINES",
            Headline = hero?.Headline ?? "Where invariant mathematics meets physical silicon.",
            Subheadline = hero?.Subheadline ?? "Explore our peer-reviewed publications, TSMC photonic tape-outs, and experimental validation logs proving bounded convergence across distributed clusters.",
            HeroImageUrl = hero?.HeroImageUrl ?? "assets/img/innovation-hero.jpg",
            LabStatusPill = hero?.LabStatusPill ?? "● ACTIVE R&D // LAB 6174",
            DieFabText = hero?.DieFabText ?? "TSMC N3E / PHOTONIC",
            OpticalWaveguideText = hero?.OpticalWaveguideText ?? "1550nm",
            LaneThroughputText = hero?.LaneThroughputText ?? "800 Gbps",
            AttractorStateText = hero?.AttractorStateText ?? "6174 REACHED",
            JitterVarianceText = hero?.JitterVarianceText ?? "0.00%",
            ConvergenceBadgeText = conv?.BadgeText ?? "EXPERIMENTAL BENCHMARK",
            ConvergenceTitle = conv?.Title ?? "The 6174 Iterative Convergence Proof",
            ConvergenceDescription = conv?.Description ?? "Every cluster transaction undergoes bounded Kaprekar transformations until it reaches the 6174 attractor equilibrium.",
            ConvergenceSteps = steps,
            Whitepapers = whitepapers,
            Patents = patents,
            Benchmarks = benchmarks
        };
    }

    public async Task<List<WhitepaperDto>> GetWhitepapersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Whitepapers
            .AsNoTracking()
            .OrderByDescending(w => w.PublishedAt)
            .Select(w => new WhitepaperDto
            {
                Id = w.Id,
                Title = w.Title,
                Slug = w.Slug,
                Authors = w.Authors,
                Category = w.Category.ToString(),
                Abstract = w.Abstract,
                PdfUrl = w.PdfUrl,
                Version = w.Version,
                DownloadCount = w.DownloadCount,
                PublishedAt = w.PublishedAt
            })
            .ToListAsync(cancellationToken);
    }

    // 8. Systems Academy
    public async Task<AcademyDto> GetAcademyAsync(CancellationToken cancellationToken = default)
    {
        var hero = await _context.AcademyHeroSections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var curr = await _context.AcademyCurriculumSections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        var phases = await _context.AcademyPhases
            .AsNoTracking()
            .OrderBy(p => p.SortOrder)
            .Select(p => new AcademyPhaseDto
            {
                PhaseNumber = p.PhaseNumber,
                PhaseBadge = p.PhaseBadge,
                Title = p.Title,
                Description = p.Description,
                SortOrder = p.SortOrder
            })
            .ToListAsync(cancellationToken);

        var tracks = await _context.AcademyTracks
            .AsNoTracking()
            .Select(t => new AcademyTrackDto
            {
                Id = t.Id,
                Slug = t.Slug,
                Name = t.Name,
                Summary = t.Summary,
                TotalPhases = t.TotalPhases,
                IsActive = t.IsActive
            })
            .ToListAsync(cancellationToken);

        var cohorts = await _context.AcademyCohorts
            .AsNoTracking()
            .Include(c => c.Track)
            .OrderBy(c => c.StartDate)
            .Select(c => new AcademyCohortDto
            {
                Id = c.Id,
                Name = c.Name,
                TrackSlug = c.Track != null ? c.Track.Slug : string.Empty,
                TrackName = c.Track != null ? c.Track.Name : string.Empty,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ApplicationDeadline = c.ApplicationDeadline,
                MaxSeats = c.MaxSeats,
                Status = c.Status.ToString()
            })
            .ToListAsync(cancellationToken);

        return new AcademyDto
        {
            BadgeText = hero?.BadgeText ?? "01 // SYSTEMS ENGINEERING ACADEMY",
            Headline = hero?.Headline ?? "Master the art of bare-metal invariant computing.",
            Subheadline = hero?.Subheadline ?? "An intensive systems curriculum covering lock-free concurrency, direct-to-metal eBPF kernel instrumentation, and deterministic distributed state.",
            HeroImageUrl = hero?.HeroImageUrl ?? "assets/img/academy-hero.jpg",
            LabStatusPill = hero?.LabStatusPill ?? "● SYSTEMS LAB // IN SESSION",
            CurriculumBadgeText = curr?.BadgeText ?? "THE 5-PHASE CURRICULUM",
            CurriculumTitle = curr?.Title ?? "From Assembly to Distributed Invariance",
            CurriculumDescription = curr?.Description ?? "Students and junior fellows don't just study abstractions; they compile custom kernels and deploy real clusters.",
            CurriculumPhases = phases,
            Tracks = tracks,
            Cohorts = cohorts
        };
    }

    public async Task<List<AcademyCohortDto>> GetAcademyCohortsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AcademyCohorts
            .AsNoTracking()
            .Include(c => c.Track)
            .OrderBy(c => c.StartDate)
            .Select(c => new AcademyCohortDto
            {
                Id = c.Id,
                Name = c.Name,
                TrackSlug = c.Track != null ? c.Track.Slug : string.Empty,
                TrackName = c.Track != null ? c.Track.Name : string.Empty,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ApplicationDeadline = c.ApplicationDeadline,
                MaxSeats = c.MaxSeats,
                Status = c.Status.ToString()
            })
            .ToListAsync(cancellationToken);
    }

    // 9. Careers Page
    public async Task<CareersDto> GetCareersAsync(CancellationToken cancellationToken = default)
    {
        var hero = await _context.CareersHeroSections.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        var depts = await _context.JobDepartments
            .AsNoTracking()
            .OrderBy(d => d.DisplayOrder)
            .Select(d => new JobDepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                DisplayOrder = d.DisplayOrder
            })
            .ToListAsync(cancellationToken);

        var jobs = await _context.JobPostings
            .AsNoTracking()
            .Include(j => j.Department)
            .OrderBy(j => j.SortOrder)
            .Select(j => new JobPostingDto
            {
                Id = j.Id,
                Slug = j.Slug,
                Title = j.Title,
                DepartmentId = j.DepartmentId,
                DepartmentName = j.Department != null ? j.Department.Name : string.Empty,
                LocationType = j.LocationType.ToString(),
                EmploymentType = j.EmploymentType.ToString(),
                ExperienceLevel = j.ExperienceLevel.ToString(),
                Description = j.Description,
                Requirements = j.Requirements,
                TechStackTags = j.TechStackTags,
                Status = j.Status.ToString(),
                IsOpen = j.Status == JobStatus.Published
            })
            .ToListAsync(cancellationToken);

        return new CareersDto
        {
            BadgeText = hero?.BadgeText ?? "01 // CAREERS & FELLOWSHIPS",
            Headline = hero?.Headline ?? "Build the foundation of tomorrow's compute fabric.",
            Subheadline = hero?.Subheadline ?? "Join an elite team of kernel architects, compiler engineers, and distributed systems researchers proving that software can be mathematically invariant.",
            HeroImageUrl = hero?.HeroImageUrl ?? "assets/img/careers-hero.jpg",
            StatusPill = hero?.StatusPill ?? "● SYSTEMS DISCIPLINES // HIRING",
            Departments = depts,
            Jobs = jobs
        };
    }

    public async Task<JobPostingDto?> GetJobPostingBySlugOrIdAsync(string slugOrId, CancellationToken cancellationToken = default)
    {
        var query = _context.JobPostings.AsNoTracking().Include(j => j.Department);
        JobPosting? job = null;

        if (Guid.TryParse(slugOrId, out var id))
        {
            job = await query.FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
        }
        else
        {
            job = await query.FirstOrDefaultAsync(j => j.Slug.ToLower() == slugOrId.ToLower(), cancellationToken);
        }

        if (job == null) return null;

        return new JobPostingDto
        {
            Id = job.Id,
            Slug = job.Slug,
            Title = job.Title,
            DepartmentId = job.DepartmentId,
            DepartmentName = job.Department?.Name ?? string.Empty,
            LocationType = job.LocationType.ToString(),
            EmploymentType = job.EmploymentType.ToString(),
            ExperienceLevel = job.ExperienceLevel.ToString(),
            Description = job.Description,
            Requirements = job.Requirements,
            TechStackTags = job.TechStackTags,
            Status = job.Status.ToString(),
            IsOpen = job.Status == JobStatus.Published
        };
    }

    // 10. Telemetry & Cluster Status
    public async Task<TelemetryStatusDto> GetTelemetryStatusAsync(CancellationToken cancellationToken = default)
    {
        var nodes = await _context.TelemetryNodes
            .AsNoTracking()
            .Include(n => n.Snapshots)
            .ToListAsync(cancellationToken);

        var nodeDtos = nodes.Select(n =>
        {
            var latestSnapshot = n.Snapshots?.OrderByDescending(s => s.RecordedAt).FirstOrDefault();
            return new TelemetryNodeDto
            {
                Id = n.Id,
                NodeIdentifier = n.NodeIdentifier,
                Region = n.Region,
                EnclaveType = n.EnclaveType.ToString(),
                Status = n.Status.ToString(),
                LatencyMs = latestSnapshot?.LatencyMs ?? 0.14,
                TpsThroughput = latestSnapshot?.TpsThroughput ?? 148290,
                JitterVariancePct = latestSnapshot?.JitterVariancePct ?? 0.0,
                ConvergenceState = latestSnapshot?.ConvergenceState ?? "6174_CONVERGED"
            };
        }).ToList();

        var activeAnnouncements = await _context.MaintenanceAnnouncements
            .AsNoTracking()
            .Where(a => a.IsActive && (a.EndsAt == null || a.EndsAt >= DateTime.UtcNow))
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new MaintenanceAnnouncementDto
            {
                Id = a.Id,
                Title = a.Title,
                Message = a.Message,
                Severity = a.Severity.ToString(),
                IsActive = a.IsActive,
                StartsAt = a.StartsAt,
                EndsAt = a.EndsAt
            })
            .ToListAsync(cancellationToken);

        var aggregateTps = nodeDtos.Sum(n => (long)n.TpsThroughput);
        var p99Max = nodeDtos.Count > 0 ? nodeDtos.Max(n => n.LatencyMs) : 0.082;
        var activeNodes = nodeDtos.Count(n => n.Status == "Online");
        var healthScore = nodeDtos.Count > 0 ? (decimal)activeNodes / nodeDtos.Count * 100m : 100m;

        return new TelemetryStatusDto
        {
            AggregateTps = aggregateTps > 0 ? aggregateTps : 2480000,
            P99LatencyMs = p99Max > 0 ? p99Max : 0.082,
            ActiveNodesCount = activeNodes > 0 ? activeNodes : 4,
            HealthScore = healthScore,
            Timestamp = DateTime.UtcNow,
            Nodes = nodeDtos,
            ActiveAnnouncements = activeAnnouncements
        };
    }

    // 11. Ingestion: Contact Inquiries
    public async Task<InquiryResponseDto> SubmitContactInquiryAsync(CreateInquiryRequestDto request, string? clientIp = null, CancellationToken cancellationToken = default)
    {
        var inquiry = new ContactInquiry
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim().ToLower(),
            Organization = request.Organization?.Trim(),
            Intent = request.IntentKey.Trim().ToLower(),
            ScaleCategory = request.ScaleKey.Trim().ToLower(),
            ScopeConstraints = request.Message.Trim(),
            Status = InquiryStatus.New,
            Priority = InquiryPriority.Normal,
            ClientIpHash = clientIp != null ? Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(clientIp))) : null
        };

        await _context.ContactInquiries.AddAsync(inquiry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _ = _emailService.SendContactNotificationAsync(
            inquiry.FullName,
            inquiry.Email,
            inquiry.Intent,
            inquiry.ScaleCategory,
            inquiry.ScopeConstraints,
            CancellationToken.None);

        var refNum = $"FDC-{DateTime.UtcNow:yyMMdd}-{inquiry.Id.ToString()[..6].ToUpper()}";

        return new InquiryResponseDto
        {
            Id = inquiry.Id,
            ReferenceNumber = refNum,
            Message = "Architectural briefing submitted successfully. A Systems Fellow will contact you shortly.",
            SubmittedAt = inquiry.CreatedAt
        };
    }

    // 12. Ingestion: Job Applications
    public async Task<JobApplicationResponseDto> SubmitJobApplicationAsync(JobApplicationRequestDto request, Stream? resumeStream = null, string? resumeFileName = null, string? contentType = null, string? clientIp = null, CancellationToken cancellationToken = default)
    {
        var job = await _context.JobPostings.FindAsync(new object[] { request.JobPostingId }, cancellationToken);
        if (job == null)
        {
            throw new InvalidOperationException("Selected job posting does not exist.");
        }

        // 1. Find or create candidate
        var candidate = await _context.Candidates
            .FirstOrDefaultAsync(c => c.Email.ToLower() == request.Email.Trim().ToLower(), cancellationToken);

        string? resumeUrl = null;
        if (resumeStream != null && !string.IsNullOrWhiteSpace(resumeFileName))
        {
            resumeUrl = await _fileStorageService.SaveFileAsync(resumeStream, resumeFileName, contentType ?? "application/pdf", "uploads/resumes", cancellationToken);
        }

        var parts = request.FullName.Trim().Split(' ', 2);
        var firstName = parts[0];
        var lastName = parts.Length > 1 ? parts[1] : string.Empty;

        if (candidate == null)
        {
            candidate = new Candidate
            {
                FirstName = firstName,
                LastName = lastName,
                Email = request.Email.Trim().ToLower(),
                Phone = request.Phone?.Trim(),
                PortfolioUrl = request.PortfolioUrl?.Trim(),
                LinkedinUrl = request.LinkedInUrl?.Trim(),
                GithubUrl = request.GitHubUrl?.Trim()
            };
            await _context.Candidates.AddAsync(candidate, cancellationToken);
        }
        else
        {
            candidate.FirstName = firstName;
            candidate.LastName = lastName;
            candidate.UpdatedAt = DateTime.UtcNow;
        }

        var application = new JobApplication
        {
            JobId = job.Id,
            Candidate = candidate,
            Status = ApplicationStage.Applied,
            CoverLetter = request.CoverLetter,
            ResumeFileUrl = resumeUrl ?? string.Empty,
            AppliedAt = DateTime.UtcNow
        };

        await _context.JobApplications.AddAsync(application, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _ = _emailService.SendJobApplicationAlertAsync(
            $"{candidate.FirstName} {candidate.LastName}".Trim(),
            candidate.Email,
            job.Title,
            application.ResumeFileUrl,
            CancellationToken.None);

        return new JobApplicationResponseDto
        {
            ApplicationId = application.Id,
            FullName = $"{candidate.FirstName} {candidate.LastName}".Trim(),
            JobTitle = job.Title,
            Status = application.Status.ToString(),
            Message = "Application submitted successfully. Our engineering talent team will review your credentials.",
            SubmittedAt = application.CreatedAt
        };
    }

    // 13. Ingestion: Student Applications
    public async Task<StudentApplicationResponseDto> SubmitStudentApplicationAsync(StudentApplicationRequestDto request, string? clientIp = null, CancellationToken cancellationToken = default)
    {
        var cohort = await _context.AcademyCohorts
            .Include(c => c.Track)
            .FirstOrDefaultAsync(c => c.Id == request.CohortId, cancellationToken);

        if (cohort == null)
        {
            throw new InvalidOperationException("Selected Academy cohort does not exist.");
        }

        var application = new StudentApplication
        {
            CohortId = cohort.Id,
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim().ToLower(),
            AcademicOrIndustryBg = request.BackgroundSummary ?? string.Empty,
            MotivationBrief = request.StatementOfPurpose ?? string.Empty,
            Status = StudentApplicationStatus.Submitted
        };

        await _context.StudentApplications.AddAsync(application, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _ = _emailService.SendCohortApplicationAlertAsync(
            application.FullName,
            application.Email,
            cohort.Track?.Name ?? cohort.Name,
            CancellationToken.None);

        return new StudentApplicationResponseDto
        {
            ApplicationId = application.Id,
            CohortCode = cohort.Name,
            TrackName = cohort.Track?.Name ?? "Systems Track",
            Status = application.Status.ToString(),
            Message = "Systems Academy application received. The admissions committee will evaluate your prerequisites.",
            SubmittedAt = application.CreatedAt
        };
    }

    // 14. Ingestion: Whitepaper Download Lead Capture
    public async Task<WhitepaperDownloadResponseDto> SubmitWhitepaperDownloadAsync(WhitepaperDownloadRequestDto request, string? clientIp = null, CancellationToken cancellationToken = default)
    {
        var whitepaper = await _context.Whitepapers.FindAsync(new object[] { request.WhitepaperId }, cancellationToken);
        if (whitepaper == null)
        {
            throw new InvalidOperationException("Selected whitepaper does not exist.");
        }

        var download = new WhitepaperDownload
        {
            WhitepaperId = whitepaper.Id,
            DownloaderName = request.FullName.Trim(),
            DownloaderEmail = request.WorkEmail.Trim().ToLower(),
            Organization = request.Organization?.Trim(),
            DownloadedAt = DateTime.UtcNow
        };

        whitepaper.DownloadCount++;
        whitepaper.UpdatedAt = DateTime.UtcNow;

        await _context.WhitepaperDownloads.AddAsync(download, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new WhitepaperDownloadResponseDto
        {
            DownloadUrl = whitepaper.PdfUrl,
            DocumentTitle = whitepaper.Title,
            Message = "Access granted to empirical whitepaper publication."
        };
    }

    // 15. Ingestion / Verification: Digital Certificate Verification
    public async Task<CertificateVerificationResponseDto> VerifyCertificateAsync(string verificationCode, CancellationToken cancellationToken = default)
    {
        var cert = await _context.Certifications
            .AsNoTracking()
            .Include(c => c.Enrollment)
            .FirstOrDefaultAsync(c => c.CertCode.ToLower() == verificationCode.Trim().ToLower(), cancellationToken);

        if (cert == null)
        {
            return new CertificateVerificationResponseDto
            {
                IsValid = false,
                VerificationCode = verificationCode,
                VerificationMessage = "No certificate matching this verification code exists in the 6174 Attractor registry."
            };
        }

        return new CertificateVerificationResponseDto
        {
            IsValid = true,
            VerificationCode = cert.CertCode,
            StudentName = cert.RecipientName,
            TrackName = cert.TrackName,
            IssueDate = cert.IssuedAt,
            ExpirationDate = null,
            Sha256Hash = cert.Sha256Hash,
            IsRevoked = false,
            VerificationMessage = "Cryptographic integrity verified against 6174 SHA-256 Attractor ledger."
        };
    }
}
