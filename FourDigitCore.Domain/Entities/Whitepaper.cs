using FourDigitCore.Domain.Common;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Domain.Entities;

public class Whitepaper : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Authors { get; set; } = "Dr. Vance K. Thorne et al.";
    public WhitepaperCategory Category { get; set; } = WhitepaperCategory.MathematicalProofs;
    public string Abstract { get; set; } = string.Empty;
    public string PdfUrl { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0";
    public int DownloadCount { get; set; } = 0;
    public bool IsPublished { get; set; } = true;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public ICollection<WhitepaperDownload> Downloads { get; set; } = new List<WhitepaperDownload>();
}

public class WhitepaperDownload : BaseEntity
{
    public Guid WhitepaperId { get; set; }
    public Whitepaper? Whitepaper { get; set; }
    public string DownloaderName { get; set; } = string.Empty;
    public string DownloaderEmail { get; set; } = string.Empty;
    public string? Organization { get; set; }
    public DateTime DownloadedAt { get; set; } = DateTime.UtcNow;
}
