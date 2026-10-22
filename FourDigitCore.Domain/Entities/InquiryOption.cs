using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class InquiryIntentOption : BaseEntity
{
    public string IntentKey { get; set; } = string.Empty;   // "cluster", "enclave", "academy", "careers", "ai", "cloud", "security", "partner"
    public string DisplayName { get; set; } = string.Empty; // "Deploy Cluster", "Hardware Enclave", "Academy Partnership", "Join Engineering"
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}

public class InquiryScaleOption : BaseEntity
{
    public string ScaleKey { get; set; } = string.Empty;    // "cluster", "enterprise", "sovereign", "academic"
    public string DisplayText { get; set; } = string.Empty; // "Cluster (< 100k TPS / Sub-millisecond)", "Enterprise Mesh (100k - 1M TPS)", etc.
    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
