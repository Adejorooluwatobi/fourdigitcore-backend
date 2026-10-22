using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class HomeHeroSection : BaseEntity
{
    public string BadgeText { get; set; } = "COMPUTATIONAL INVARIANCE // 6174";
    public string Headline { get; set; } = "Architecting Mathematical Determinism at Infinite Scale.";
    public string Subheadline { get; set; } = "Direct-to-metal agentic execution graphs, sub-millisecond tensor streaming, and self-stabilizing cloud clusters governed by Kaprekar invariance.";
    public string PrimaryCtaText { get; set; } = "Deploy Architecture →";
    public string PrimaryCtaUrl { get; set; } = "contact.html";
    public string SecondaryCtaText { get; set; } = "Inspect Kernels";
    public string SecondaryCtaUrl { get; set; } = "#strata";
    public string LatencyCeilingValue { get; set; } = "< 0.18ms";
    public string LatencyCeilingLabel { get; set; } = "LATENCY CEILING";
    public string StateModelValue { get; set; } = "Kaprekar 6174";
    public string StateModelLabel { get; set; } = "STATE MODEL";
    public string EnclaveSecurityValue { get; set; } = "FIPS 140-3 L4";
    public string EnclaveSecurityLabel { get; set; } = "ENCLAVE SECURITY";
    public ICollection<HomeHeroSlide> Slides { get; set; } = new List<HomeHeroSlide>();
}

public class HomeHeroSlide : BaseEntity
{
    public Guid HeroSectionId { get; set; }
    public HomeHeroSection? HeroSection { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public int SortOrder { get; set; } = 0;
}
