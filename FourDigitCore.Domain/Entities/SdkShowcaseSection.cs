using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class SdkShowcaseSection : BaseEntity
{
    public string SectionKey { get; set; } = "home_sdk";
    public string BadgeText { get; set; } = "04 // DEVELOPER ECOSYSTEM";
    public string Title { get; set; } = "Native SDKs for Ultra-Low Latency";
    public string Description { get; set; } = "Deploy deterministic cluster connections in under 4 lines of code. Zero unneeded runtime abstractions.";
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; } = 0;
    public ICollection<SdkCodeSnippet> Snippets { get; set; } = new List<SdkCodeSnippet>();
}
