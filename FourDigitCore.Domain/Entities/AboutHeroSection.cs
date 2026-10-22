using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class AboutHeroSection : BaseEntity
{
    public string BadgeText { get; set; } = "01 // PHILOSOPHY & MATHEMATICAL DETERMINISM";
    public string Headline { get; set; } = "Founded on the principle of Kaprekar Invariance.";
    public string Subheadline { get; set; } = "In an industry that tolerates unbounded complexity, FourDigitCore builds on invariant mathematics. Every packet, memory page, and neural tensor converges deterministically.";
    public string HeroImageUrl { get; set; } = "assets/img/about-hero.jpg";
    public string HeroHudPill { get; set; } = "● FOUNDATION // MATHEMATICAL PROOFS";
    public string EquilibriumText { get; set; } = "6174 INVARIANT";
    public string ClusterStateText { get; set; } = "STABLE";
}
