using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class CareersHeroSection : BaseEntity
{
    public string BadgeText { get; set; } = "01 // CAREERS & FELLOWSHIPS";
    public string Headline { get; set; } = "Build the foundation of tomorrow's compute fabric.";
    public string Subheadline { get; set; } = "Join an elite team of kernel architects, compiler engineers, and distributed systems researchers proving that software can be mathematically invariant.";
    public string HeroImageUrl { get; set; } = "assets/img/careers-hero.jpg";
    public string StatusPill { get; set; } = "● SYSTEMS DISCIPLINES // HIRING";
    public string Ring0Tech { get; set; } = "RUST / eBPF";
    public string NeuralTech { get; set; } = "CUDA / FP8";
    public string EnclaveTech { get; set; } = "SEV-SNP";
    public string ConsensusTech { get; set; } = "KAPREKAR 6174";
}
