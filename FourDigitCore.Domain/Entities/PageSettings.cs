using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class PageSettings : BaseEntity
{
    public string PageKey { get; set; } = string.Empty; // "index", "solutions", "about", "innovation", "academy", "careers", "contact"
    public string SeoTitle { get; set; } = string.Empty;
    public string SeoDescription { get; set; } = string.Empty;
    public string? HeroBadgeText { get; set; }
    public string? HeroHeading { get; set; }
    public string? HeroSubtitle { get; set; }
}
