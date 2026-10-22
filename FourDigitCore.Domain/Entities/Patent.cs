using FourDigitCore.Domain.Common;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Domain.Entities;

public class Patent : BaseEntity
{
    public string PatentNumber { get; set; } = string.Empty; // e.g. "US Patent 11,849,617"
    public string Title { get; set; } = string.Empty;
    public DateTime FilingDate { get; set; }
    public DateTime? GrantDate { get; set; }
    public PatentStatus Status { get; set; } = PatentStatus.Granted;
    public string Abstract { get; set; } = string.Empty;
    public string Inventors { get; set; } = "Dr. Vance K. Thorne";
    public string? DocUrl { get; set; }
}

public class Benchmark : BaseEntity
{
    public string Subsystem { get; set; } = string.Empty; // "FP8 Tensor Streaming", "Consensus Attractor"
    public string WorkloadType { get; set; } = string.Empty;
    public long ThroughputTps { get; set; }
    public double LatencyP50Ms { get; set; }
    public double LatencyP99Ms { get; set; }
    public int InvarianceCycles { get; set; } = 7;
    public DateTime BenchmarkDate { get; set; } = DateTime.UtcNow;
    public string? ReproducibilityRepoUrl { get; set; }
}
