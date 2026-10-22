using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class HomeClosingCallout : BaseEntity
{
    public string BadgeText { get; set; } = "ENTERPRISE DEPLOYMENTS";
    public string Title { get; set; } = "Have an infrastructure challenge worth solving?";
    public string Description { get; set; } = "Connect with our kernel infrastructure team to deploy dedicated sovereign clusters, hybrid-enclave meshes, or on-premise appliances.";
    public string PrimaryCtaText { get; set; } = "Launch Console →";
    public string PrimaryCtaUrl { get; set; } = "contact.html";
    public string SecondaryCtaText { get; set; } = "Read Whitepaper";
    public string SecondaryCtaUrl { get; set; } = "innovation.html";
}
