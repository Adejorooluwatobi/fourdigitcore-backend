using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class HomeQuoteSection : BaseEntity
{
    public string QuoteText { get; set; } = "Software engineering is not about accumulating complexity; it is about finding the invariant core that remains true across infinite scale.";
    public string Attribution { get; set; } = "KAPREKAR 6174 PRINCIPLE";
    public string InvariantTerm { get; set; } = "invariant core";
}
