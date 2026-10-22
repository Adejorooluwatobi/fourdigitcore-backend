using FourDigitCore.Domain.Common;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Domain.Entities;

public class StudentApplication : BaseEntity
{
    public Guid CohortId { get; set; }
    public AcademyCohort? Cohort { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string GithubProfile { get; set; } = string.Empty;
    public string AcademicOrIndustryBg { get; set; } = string.Empty;
    public string MotivationBrief { get; set; } = string.Empty;
    public StudentApplicationStatus Status { get; set; } = StudentApplicationStatus.Submitted;
}

public class StudentEnrollment : BaseEntity
{
    public Guid CohortId { get; set; }
    public AcademyCohort? Cohort { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public int CurrentPhaseNumber { get; set; } = 1;
    public bool IsGraduated { get; set; } = false;
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public Certification? Certification { get; set; }
}

public class Certification : BaseEntity
{
    public Guid EnrollmentId { get; set; }
    public StudentEnrollment? Enrollment { get; set; }
    public string CertCode { get; set; } = string.Empty;       // e.g. "6174-CERT-9A82D1"
    public string RecipientName { get; set; } = string.Empty;
    public string TrackName { get; set; } = string.Empty;
    public string Sha256Hash { get; set; } = string.Empty;     // Cryptographic signature
    public string SignedByAdminName { get; set; } = "Dr. Vance K. Thorne";
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public string VerificationUrl { get; set; } = string.Empty;
}
