using FluentAssertions;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.Services;
using FourDigitCore.Domain.Entities;
using FourDigitCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FourDigitCore.Test.Services;

public class DynamicContentTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly Mock<IFileStorageService> _fileStorageMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly FourDigitCorePublicService _publicService;

    public DynamicContentTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
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
    public async Task GetSiteSettingsAsync_ShouldReturnDynamicConfiguredSettings()
    {
        // Arrange
        var settings = new SiteSettings
        {
            SiteTitle = "6174 Attractor Core | Deterministic Convergence Engine",
            Monogram = "6174",
            LogoLightUrl = "assets/img/logo-light.png",
            LogoDarkUrl = "assets/img/logo-dark.png",
            FaviconUrl = "assets/img/favicon.png",
            DefaultTheme = "dark",
            MetaDescription = "Autonomous compute fabric engineered for high-throughput AI orchestration.",
            CopyrightText = "© 2026 FourDigitCore Inc. All rights reserved.",
            FooterTagline = "Distributed Systems Engineered for Invariant Equilibrium",
            KernelStatusBadge = "KERNEL//SEC_LEVEL_0",
            LatencyTickerText = "LATENCY: 0.14ms",
            ActiveClusterText = "GLOBAL CLUSTER ACTIVE",
            InvarianceObservedText = "6174 INVARIANCE OBSERVED"
        };
        await _context.SiteSettings.AddAsync(settings);
        await _context.SaveChangesAsync();

        // Act
        var result = await _publicService.GetSiteSettingsAsync();

        // Assert
        result.Should().NotBeNull();
        result.SiteTitle.Should().Be("6174 Attractor Core | Deterministic Convergence Engine");
        result.Monogram.Should().Be("6174");
        result.FooterTagline.Should().Be("Distributed Systems Engineered for Invariant Equilibrium");
        result.CopyrightText.Should().Be("© 2026 FourDigitCore Inc. All rights reserved.");
        result.KernelStatusBadge.Should().Be("KERNEL//SEC_LEVEL_0");
    }

    [Fact]
    public async Task GetNavigationAsync_ShouldReturnOrderedNavItemsAndFooters()
    {
        // Arrange
        var nav1 = new NavItem { Label = "Architecture", Url = "/#architecture", DataPage = "home", SortOrder = 2, IsActive = true };
        var nav2 = new NavItem { Label = "SDK", Url = "/#sdk", DataPage = "home", SortOrder = 1, IsActive = true };
        var navInactive = new NavItem { Label = "Hidden", Url = "/hidden", DataPage = "hidden", SortOrder = 3, IsActive = false };

        var footerCol = new FooterColumn { Title = "ECOSYSTEM", SortOrder = 1 };
        footerCol.Links.Add(new FooterLink { Label = "Academy", Url = "/academy.html", SortOrder = 1 });
        footerCol.Links.Add(new FooterLink { Label = "Whitepapers", Url = "/innovation.html#whitepapers", SortOrder = 2 });

        await _context.NavItems.AddRangeAsync(nav1, nav2, navInactive);
        await _context.FooterColumns.AddAsync(footerCol);
        await _context.SaveChangesAsync();

        // Act
        var result = await _publicService.GetNavigationAsync();

        // Assert
        result.Should().NotBeNull();
        result.HeaderNav.Should().HaveCount(2);
        result.HeaderNav[0].Label.Should().Be("SDK"); // SortOrder = 1
        result.HeaderNav[1].Label.Should().Be("Architecture"); // SortOrder = 2

        result.FooterColumns.Should().HaveCount(1);
        result.FooterColumns[0].Title.Should().Be("ECOSYSTEM");
        result.FooterColumns[0].Links.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetSolutionsAsync_ShouldReturnFullDisciplineSpecsAndApplications()
    {
        // Arrange
        var hero = new SolutionsHeroSection
        {
            BadgeText = "MISSION-CRITICAL DISCIPLINES",
            Headline = "Engineering High-Entropy Invariance",
            Subheadline = "Zero-overhead distributed consensus"
        };
        await _context.SolutionsHeroSections.AddAsync(hero);

        var discipline = new SolutionDiscipline
        {
            DisciplineNumber = "01",
            CategoryName = "HIGH-FREQUENCY TRADING",
            Title = "Ultra-Low-Latency Trading Invariance",
            Description = "Deterministic matching kernels operating under 14ns latency.",
            SortOrder = 1
        };
        discipline.Specs.Add(new SolutionDisciplineSpec
        {
            SpecText = "< 18ns Latency Ceiling",
            SortOrder = 1
        });
        discipline.Applications.Add(new SolutionDisciplineApplication
        {
            ApplicationName = "Dark Pool Settlement Attractor",
            SortOrder = 1
        });

        await _context.SolutionDisciplines.AddAsync(discipline);
        await _context.SaveChangesAsync();

        // Act
        var result = await _publicService.GetSolutionsAsync();

        // Assert
        result.Should().NotBeNull();
        result.BadgeText.Should().Be("MISSION-CRITICAL DISCIPLINES");
        result.Headline.Should().Be("Engineering High-Entropy Invariance");
        result.Disciplines.Should().HaveCount(1);
        result.Disciplines[0].DisciplineNumber.Should().Be("01");
        result.Disciplines[0].Specs.Should().HaveCount(1);
        result.Disciplines[0].Specs[0].SpecText.Should().Be("< 18ns Latency Ceiling");
        result.Disciplines[0].Applications.Should().HaveCount(1);
        result.Disciplines[0].Applications[0].ApplicationName.Should().Be("Dark Pool Settlement Attractor");
    }
}
