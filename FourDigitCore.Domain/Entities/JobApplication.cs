using FourDigitCore.Domain.Common;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Domain.Entities;

public class Candidate : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? LinkedinUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string? PortfolioUrl { get; set; }
    public string? CurrentCompany { get; set; }
    public string? Location { get; set; }
    public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
}

public class JobApplication : BaseEntity
{
    public Guid JobId { get; set; }
    public JobPosting? Job { get; set; }
    public Guid CandidateId { get; set; }
    public Candidate? Candidate { get; set; }
    public string ResumeFileUrl { get; set; } = string.Empty;
    public string? CoverLetter { get; set; }
    public ApplicationStage Status { get; set; } = ApplicationStage.Applied;
    public int Rating { get; set; } = 0; // 1 to 5 stars
    public string? RejectionReason { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ApplicationNote> Notes { get; set; } = new List<ApplicationNote>();
    public ICollection<ApplicationInterview> Interviews { get; set; } = new List<ApplicationInterview>();
}

public class ApplicationNote : BaseEntity
{
    public Guid ApplicationId { get; set; }
    public JobApplication? Application { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int? Rating { get; set; }
}

public class ApplicationInterview : BaseEntity
{
    public Guid ApplicationId { get; set; }
    public JobApplication? Application { get; set; }
    public string InterviewerName { get; set; } = string.Empty;
    public string StageName { get; set; } = string.Empty; // e.g. "Ring 0 Live Coding"
    public DateTime ScheduledAt { get; set; }
    public string? MeetingLink { get; set; }
    public string? FeedbackSummary { get; set; }
    public string Outcome { get; set; } = "Scheduled"; // Scheduled, Passed, Failed, Cancelled
}
