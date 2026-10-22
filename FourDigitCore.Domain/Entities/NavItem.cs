using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class NavItem : BaseEntity
{
    public string Label { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string DataPage { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
