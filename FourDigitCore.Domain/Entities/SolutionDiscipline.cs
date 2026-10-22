using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class SolutionDiscipline : BaseEntity
{
    public string DisciplineNumber { get; set; } = string.Empty; // "01", "02", "03", "04"
    public string CategoryName { get; set; } = string.Empty;    // "01 // NEURAL RUNTIMES & ACCELERATION"
    public string Title { get; set; } = string.Empty;           // "Autonomous Neural Kernels"
    public string Description { get; set; } = string.Empty;
    public string CtaText { get; set; } = "Request Kernel Benchmark →";
    public string CtaUrl { get; set; } = "contact.html?intent=ai";
    public int SortOrder { get; set; } = 0;
    public ICollection<SolutionDisciplineSpec> Specs { get; set; } = new List<SolutionDisciplineSpec>();
    public ICollection<SolutionDisciplineApplication> Applications { get; set; } = new List<SolutionDisciplineApplication>();
}

public class SolutionDisciplineSpec : BaseEntity
{
    public Guid DisciplineId { get; set; }
    public SolutionDiscipline? Discipline { get; set; }
    public string SpecText { get; set; } = string.Empty; // "FP8 Tensor Streaming", "Sub-0.18ms Latency"
    public int SortOrder { get; set; } = 0;
}

public class SolutionDisciplineApplication : BaseEntity
{
    public Guid DisciplineId { get; set; }
    public SolutionDiscipline? Discipline { get; set; }
    public string ApplicationName { get; set; } = string.Empty; // "Autonomous Agents", "High-Frequency Trading"
    public int SortOrder { get; set; } = 0;
}
