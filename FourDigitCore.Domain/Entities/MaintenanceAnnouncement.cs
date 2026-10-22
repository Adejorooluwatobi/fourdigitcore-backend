using FourDigitCore.Domain.Common;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Domain.Entities;

public class MaintenanceAnnouncement : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public SeverityLevel Severity { get; set; } = SeverityLevel.Info;
    public bool IsActive { get; set; } = false;
    public DateTime StartsAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndsAt { get; set; }
}
