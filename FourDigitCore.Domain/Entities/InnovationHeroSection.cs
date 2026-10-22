using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class InnovationHeroSection : BaseEntity
{
    public string BadgeText { get; set; } = "01 // RESEARCH LABS & PROOF ENGINES";
    public string Headline { get; set; } = "Where invariant mathematics meets physical silicon.";
    public string Subheadline { get; set; } = "Explore our peer-reviewed publications, TSMC photonic tape-outs, and experimental validation logs proving bounded convergence across distributed clusters.";
    public string HeroImageUrl { get; set; } = "assets/img/innovation-hero.jpg";
    public string LabStatusPill { get; set; } = "● ACTIVE R&D // LAB 6174";
    public string DieFabText { get; set; } = "TSMC N3E / PHOTONIC";
    public string OpticalWaveguideText { get; set; } = "1550nm";
    public string LaneThroughputText { get; set; } = "800 Gbps";
    public string AttractorStateText { get; set; } = "6174 REACHED";
    public string JitterVarianceText { get; set; } = "0.00%";
}
