using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class PlatformPillar : BaseEntity
{
    public string LayerNumber { get; set; } = string.Empty; // "01", "02", "03"
    public string BadgeText { get; set; } = string.Empty;   // "HARDWARE ENCLAVES", etc.
    public string Title { get; set; } = string.Empty;       // "Silicon-Rooted Enclaves"
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;        // "security", "hub", "bolt"
    public int SortOrder { get; set; } = 0;
    public ICollection<PlatformPillarSpec> Specs { get; set; } = new List<PlatformPillarSpec>();
    public ICollection<PlatformPillarApplication> Applications { get; set; } = new List<PlatformPillarApplication>();
}

public class PlatformPillarSpec : BaseEntity
{
    public Guid PillarId { get; set; }
    public PlatformPillar? Pillar { get; set; }
    public string SpecText { get; set; } = string.Empty;
    public int SortOrder { get; set; } = 0;
}

public class PlatformPillarApplication : BaseEntity
{
    public Guid PillarId { get; set; }
    public PlatformPillar? Pillar { get; set; }
    public string ApplicationName { get; set; } = string.Empty;
    public int SortOrder { get; set; } = 0;
}
