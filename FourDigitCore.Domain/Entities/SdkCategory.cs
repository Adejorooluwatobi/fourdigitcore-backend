using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class SdkCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty; // e.g. "Core Systems", "Application & Mobile Runtimes"
    public string Slug { get; set; } = string.Empty;
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public ICollection<SdkCodeSnippet> Snippets { get; set; } = new List<SdkCodeSnippet>();
}
