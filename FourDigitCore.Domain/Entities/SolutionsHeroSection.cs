using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class SolutionsHeroSection : BaseEntity
{
    public string BadgeText { get; set; } = "01 // SOLUTIONS & ARCHITECTURAL DISCIPLINES";
    public string Headline { get; set; } = "Technology engineered around deterministic scale.";
    public string Subheadline { get; set; } = "From direct-to-metal neural kernels to post-quantum zero-trust enclaves and distributed mesh fabrics, we engineer systems that remain invariant under infinite load.";
    public string HeroImageUrl { get; set; } = "assets/img/solutions-hero.jpg";
    public string TopologyStatusText { get; set; } = "● TOPOLOGY // 4/4 STRATA ACTIVE";
    public string LatencyCeilingValue { get; set; } = "< 0.18ms";
    public string LatencyCeilingLabel { get; set; } = "LATENCY CEILING";
    public string StateModelValue { get; set; } = "Kaprekar 6174";
    public string StateModelLabel { get; set; } = "STATE MODEL";
    public string EnclaveSecurityValue { get; set; } = "FIPS 140-3 L4";
    public string EnclaveSecurityLabel { get; set; } = "ENCLAVE SECURITY";
}
