namespace FourDigitCore.Application.DTOs.Public;

public class CreateInquiryRequestDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Organization { get; set; }
    public string IntentKey { get; set; } = string.Empty;
    public string ScaleKey { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class InquiryResponseDto
{
    public Guid Id { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
}

public class JobApplicationRequestDto
{
    public Guid JobPostingId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? PortfolioUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CoverLetter { get; set; }
}

public class JobApplicationResponseDto
{
    public Guid ApplicationId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
}

public class StudentApplicationRequestDto
{
    public Guid CohortId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? BackgroundSummary { get; set; }
    public string? StatementOfPurpose { get; set; }
}

public class StudentApplicationResponseDto
{
    public Guid ApplicationId { get; set; }
    public string CohortCode { get; set; } = string.Empty;
    public string TrackName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
}

public class WhitepaperDownloadRequestDto
{
    public Guid WhitepaperId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string WorkEmail { get; set; } = string.Empty;
    public string? Organization { get; set; }
    public string? RoleTitle { get; set; }
}

public class WhitepaperDownloadResponseDto
{
    public string DownloadUrl { get; set; } = string.Empty;
    public string DocumentTitle { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class CertificateVerificationResponseDto
{
    public bool IsValid { get; set; }
    public string VerificationCode { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string TrackName { get; set; } = string.Empty;
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string Sha256Hash { get; set; } = string.Empty;
    public bool IsRevoked { get; set; }
    public string VerificationMessage { get; set; } = string.Empty;
}
