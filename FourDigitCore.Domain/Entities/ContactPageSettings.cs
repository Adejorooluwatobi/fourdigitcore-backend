using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class ContactPageSettings : BaseEntity
{
    public string BadgeText { get; set; } = "01 // ARCHITECTURAL BRIEFING CONSOLE";
    public string Headline { get; set; } = "Launch deterministic systems with FourDigitCore.";
    public string Subheadline { get; set; } = "Submit your infrastructure parameters, enclave requirements, or scale profiles. Our systems architects will respond with an executable deterministic topology.";
    public string IntentLabel { get; set; } = "SELECT ARCHITECTURAL INTENT";
    public string FeedbackSuccessText { get; set; } = "✓ Architectural briefing transmitted. Our principal systems fellow will reply within 4 hours.";
}
