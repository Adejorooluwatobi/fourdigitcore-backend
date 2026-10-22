namespace FourDigitCore.Domain.Common;

/// <summary>
/// Universal Base Entity for all FourDigitCore domain models.
/// Uses Guid for deterministic time-ordered primary keys and UTC timestamps.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
