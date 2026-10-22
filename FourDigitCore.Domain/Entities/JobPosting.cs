using FourDigitCore.Domain.Common;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Domain.Entities;

public class JobDepartment : BaseEntity
{
    public string Name { get; set; } = string.Empty; // e.g. "Kernel Infrastructure", "Post-Quantum Cryptography", "Neural Acceleration"
    public int DisplayOrder { get; set; } = 0;
    public ICollection<JobPosting> JobPostings { get; set; } = new List<JobPosting>();
}

public class JobPosting : BaseEntity
{
    public Guid DepartmentId { get; set; }
    public JobDepartment? Department { get; set; }
    public string Title { get; set; } = string.Empty; // e.g. "Staff Rust / eBPF Kernel Engineer"
    public string Slug { get; set; } = string.Empty;
    public LocationType LocationType { get; set; } = LocationType.Remote;
    public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;
    public ExperienceLevel ExperienceLevel { get; set; } = ExperienceLevel.Senior;
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string TechStackTags { get; set; } = "[]"; // JSON array or comma separated
    public JobStatus Status { get; set; } = JobStatus.Published;
    public int SortOrder { get; set; } = 0;
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}
