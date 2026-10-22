using FourDigitCore.Domain.Common;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Domain.Entities;

public class TelemetryNode : BaseEntity
{
    public string NodeIdentifier { get; set; } = string.Empty; // e.g. "NODE-US-EAST-01"
    public string Region { get; set; } = "US-EAST";            // "US-EAST", "EU-CENTRAL", "AP-NORTHEAST"
    public EnclaveType EnclaveType { get; set; } = EnclaveType.AmdSevSnp;
    public ClusterStatus Status { get; set; } = ClusterStatus.Online;
    public ICollection<TelemetrySnapshot> Snapshots { get; set; } = new List<TelemetrySnapshot>();
}

public class TelemetrySnapshot : BaseEntity
{
    public Guid NodeId { get; set; }
    public TelemetryNode? Node { get; set; }
    public double LatencyMs { get; set; } = 0.14;
    public int TpsThroughput { get; set; } = 148290;
    public double JitterVariancePct { get; set; } = 0.0;
    public string ConvergenceState { get; set; } = "6174_CONVERGED";
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}
