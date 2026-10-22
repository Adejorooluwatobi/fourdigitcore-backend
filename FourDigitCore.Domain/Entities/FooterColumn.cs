using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class FooterColumn : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int SortOrder { get; set; } = 0;
    public ICollection<FooterLink> Links { get; set; } = new List<FooterLink>();
}

public class FooterLink : BaseEntity
{
    public Guid ColumnId { get; set; }
    public FooterColumn? Column { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int SortOrder { get; set; } = 0;
}
