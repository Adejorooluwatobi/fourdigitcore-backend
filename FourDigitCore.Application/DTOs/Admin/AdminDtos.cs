namespace FourDigitCore.Application.DTOs.Admin;

// SDK Management
public class UpdateSdkSectionDto
{
    public string BadgeText { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpsertSdkCategoryDto
{
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class UpsertSdkSnippetDto
{
    public Guid? CategoryId { get; set; }
    public string LanguageKey { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string BindingTag { get; set; } = string.Empty;
    public string PackageTag { get; set; } = string.Empty;
    public string MemoryTag { get; set; } = string.Empty;
    public string ThreadsTag { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string CodeContent { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

// Inquiries CRM
public class UpdateInquiryStatusDto
{
    public string Status { get; set; } = string.Empty;
    public string? Priority { get; set; }
    public string? AssignedAdminName { get; set; }
}

public class AddInquiryNoteDto
{
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
}

// Careers & ATS
public class UpsertJobPostingDto
{
    public Guid DepartmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string LocationType { get; set; } = "Remote";
    public string EmploymentType { get; set; } = "FullTime";
    public string ExperienceLevel { get; set; } = "Senior";
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string TechStackTags { get; set; } = "[]";
    public string Status { get; set; } = "Published";
    public int SortOrder { get; set; }
}

public class UpdateApplicationStageDto
{
    public string Stage { get; set; } = string.Empty;
    public int? Rating { get; set; }
    public string? RejectionReason { get; set; }
}

public class AddApplicationNoteDto
{
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public int? Rating { get; set; }
}

public class ScheduleInterviewDto
{
    public string InterviewerName { get; set; } = string.Empty;
    public string StageName { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public string? MeetingLink { get; set; }
}

// Systems Academy
public class UpsertCohortDto
{
    public Guid TrackId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime ApplicationDeadline { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxSeats { get; set; } = 30;
    public string Status { get; set; } = "Enrolling";
}

public class UpdateStudentAppStatusDto
{
    public string Status { get; set; } = string.Empty;
}

public class IssueCertificateDto
{
    public Guid EnrollmentId { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string TrackName { get; set; } = string.Empty;
    public string SignedByAdminName { get; set; } = "Dr. Vance K. Thorne";
}

public class CertificateIssuedResponseDto
{
    public Guid CertificateId { get; set; }
    public string CertCode { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string TrackName { get; set; } = string.Empty;
    public string Sha256Hash { get; set; } = string.Empty;
    public string VerificationUrl { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
}

// Maintenance Announcements
public class UpsertAnnouncementDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = "Info";
    public bool IsActive { get; set; }
    public DateTime StartsAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndsAt { get; set; }
}

// Solutions Management
public class UpsertSolutionDisciplineDto
{
    public string DisciplineNumber { get; set; } = "01";
    public string CategoryName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CtaText { get; set; } = "Deploy Module";
    public string CtaUrl { get; set; } = "/contact";
    public int SortOrder { get; set; }
    public List<string> Specs { get; set; } = new();
    public List<string> Applications { get; set; } = new();
}

