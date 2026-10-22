using FourDigitCore.Domain.Common;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Domain.Entities;

public class AcademyTrack : BaseEntity
{
    public string Name { get; set; } = "Autonomous Compute Systems Fellowship";
    public string Slug { get; set; } = "systems-fellowship";
    public string Summary { get; set; } = "Comprehensive 5-phase systems engineering track covering low-level kernel abstractions, lock-free concurrency, and distributed invariance.";
    public int TotalPhases { get; set; } = 5;
    public bool IsActive { get; set; } = true;
    public ICollection<AcademyCohort> Cohorts { get; set; } = new List<AcademyCohort>();
}

public class AcademyCohort : BaseEntity
{
    public Guid TrackId { get; set; }
    public AcademyTrack? Track { get; set; }
    public string Name { get; set; } = string.Empty; // e.g. "Fellowship Cohort 2026-Q1"
    public DateTime ApplicationDeadline { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxSeats { get; set; } = 30;
    public CohortStatus Status { get; set; } = CohortStatus.Enrolling;
    public ICollection<StudentApplication> Applications { get; set; } = new List<StudentApplication>();
    public ICollection<StudentEnrollment> Enrollments { get; set; } = new List<StudentEnrollment>();
}
