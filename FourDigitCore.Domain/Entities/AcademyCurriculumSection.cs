using FourDigitCore.Domain.Common;

namespace FourDigitCore.Domain.Entities;

public class AcademyCurriculumSection : BaseEntity
{
    public string BadgeText { get; set; } = "THE 5-PHASE CURRICULUM";
    public string Title { get; set; } = "From Assembly to Distributed Invariance";
    public string Description { get; set; } = "Students and junior fellows don't just study abstractions; they compile custom kernels and deploy real clusters.";
    public ICollection<AcademyPhase> Phases { get; set; } = new List<AcademyPhase>();
}

public class AcademyPhase : BaseEntity
{
    public Guid CurriculumSectionId { get; set; }
    public AcademyCurriculumSection? CurriculumSection { get; set; }
    public string PhaseNumber { get; set; } = string.Empty; // "01", "02", "03", "04", "05"
    public string PhaseBadge { get; set; } = string.Empty;  // "01 // DISCOVER"
    public string Title { get; set; } = string.Empty;       // "Computer Architecture"
    public string Description { get; set; } = string.Empty; // "Memory hierarchies, CPU caches, and register allocation."
    public int SortOrder { get; set; } = 0;
}
