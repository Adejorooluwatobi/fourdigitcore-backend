using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Domain.Entities;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Application.Services;

public interface IFourDigitCoreAdminService
{
    // SDK Showcase Management
    Task<bool> UpdateSdkSectionAsync(UpdateSdkSectionDto dto, CancellationToken cancellationToken = default);
    Task<SdkCategory> CreateSdkCategoryAsync(UpsertSdkCategoryDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateSdkCategoryAsync(Guid id, UpsertSdkCategoryDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteSdkCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SdkCodeSnippet> CreateSdkSnippetAsync(UpsertSdkSnippetDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateSdkSnippetAsync(Guid id, UpsertSdkSnippetDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteSdkSnippetAsync(Guid id, CancellationToken cancellationToken = default);

    // Inquiries CRM
    Task<List<ContactInquiry>> GetInquiriesAsync(string? status = null, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
    Task<ContactInquiry?> GetInquiryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> UpdateInquiryStatusAsync(Guid id, UpdateInquiryStatusDto dto, CancellationToken cancellationToken = default);
    Task<InquiryNote?> AddInquiryNoteAsync(Guid id, AddInquiryNoteDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteInquiryAsync(Guid id, CancellationToken cancellationToken = default);

    // Careers & ATS
    Task<JobPosting> CreateJobPostingAsync(UpsertJobPostingDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateJobPostingAsync(Guid id, UpsertJobPostingDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteJobPostingAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<JobApplication>> GetJobApplicationsAsync(string? stage = null, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default);
    Task<JobApplication?> GetJobApplicationByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> UpdateApplicationStageAsync(Guid id, UpdateApplicationStageDto dto, CancellationToken cancellationToken = default);
    Task<ApplicationNote?> AddApplicationNoteAsync(Guid id, AddApplicationNoteDto dto, CancellationToken cancellationToken = default);
    Task<ApplicationInterview?> ScheduleInterviewAsync(Guid id, ScheduleInterviewDto dto, CancellationToken cancellationToken = default);

    // Systems Academy
    Task<AcademyCohort> CreateCohortAsync(UpsertCohortDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateCohortAsync(Guid id, UpsertCohortDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteCohortAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<StudentApplication>> GetStudentApplicationsAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateStudentAppStatusAsync(Guid id, UpdateStudentAppStatusDto dto, CancellationToken cancellationToken = default);
    Task<CertificateIssuedResponseDto> IssueCertificateAsync(IssueCertificateDto dto, CancellationToken cancellationToken = default);
    Task<List<Certification>> GetCertificatesAsync(CancellationToken cancellationToken = default);

    // Telemetry & Maintenance
    Task<List<TelemetryNode>> GetTelemetryNodesAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateNodeStatusAsync(Guid id, ClusterStatus status, CancellationToken cancellationToken = default);
    Task<MaintenanceAnnouncement> CreateAnnouncementAsync(UpsertAnnouncementDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAnnouncementAsync(Guid id, UpsertAnnouncementDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAnnouncementAsync(Guid id, CancellationToken cancellationToken = default);

    // Forensic Audit Trail
    Task<List<AuditLog>> GetAuditLogsAsync(int page = 1, int pageSize = 50, CancellationToken cancellationToken = default);
    Task LogActionAsync(Guid? userId, string action, string entityType, Guid? entityId, string? diffJson = null, string? clientIp = null, CancellationToken cancellationToken = default);
}

public class FourDigitCoreAdminService : IFourDigitCoreAdminService
{
    private readonly IAppDbContext _context;

    public FourDigitCoreAdminService(IAppDbContext context)
    {
        _context = context;
    }

    // 1. SDK Showcase Management
    public async Task<bool> UpdateSdkSectionAsync(UpdateSdkSectionDto dto, CancellationToken cancellationToken = default)
    {
        var section = await _context.SdkShowcaseSections.FirstOrDefaultAsync(cancellationToken);
        if (section == null)
        {
            section = new SdkShowcaseSection { SectionKey = "home_sdk" };
            await _context.SdkShowcaseSections.AddAsync(section, cancellationToken);
        }

        section.BadgeText = dto.BadgeText;
        section.Title = dto.Title;
        section.Description = dto.Description;
        section.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<SdkCategory> CreateSdkCategoryAsync(UpsertSdkCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = new SdkCategory
        {
            Slug = dto.Slug.Trim().ToLower(),
            Name = dto.Name.Trim(),
            SortOrder = dto.SortOrder
        };

        await _context.SdkCategories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<bool> UpdateSdkCategoryAsync(Guid id, UpsertSdkCategoryDto dto, CancellationToken cancellationToken = default)
    {
        var category = await _context.SdkCategories.FindAsync(new object[] { id }, cancellationToken);
        if (category == null) return false;

        category.Slug = dto.Slug.Trim().ToLower();
        category.Name = dto.Name.Trim();
        category.SortOrder = dto.SortOrder;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteSdkCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _context.SdkCategories.FindAsync(new object[] { id }, cancellationToken);
        if (category == null) return false;

        _context.SdkCategories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<SdkCodeSnippet> CreateSdkSnippetAsync(UpsertSdkSnippetDto dto, CancellationToken cancellationToken = default)
    {
        var snippet = new SdkCodeSnippet
        {
            CategoryId = dto.CategoryId,
            LanguageKey = dto.LanguageKey.Trim().ToLower(),
            DisplayName = dto.DisplayName.Trim(),
            FileName = dto.FileName.Trim(),
            BindingTag = dto.BindingTag.Trim(),
            PackageTag = dto.PackageTag.Trim(),
            MemoryTag = dto.MemoryTag.Trim(),
            ThreadsTag = dto.ThreadsTag.Trim(),
            SortOrder = dto.SortOrder,
            CodeContent = dto.CodeContent,
            IsActive = dto.IsActive
        };

        await _context.SdkCodeSnippets.AddAsync(snippet, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return snippet;
    }

    public async Task<bool> UpdateSdkSnippetAsync(Guid id, UpsertSdkSnippetDto dto, CancellationToken cancellationToken = default)
    {
        var snippet = await _context.SdkCodeSnippets.FindAsync(new object[] { id }, cancellationToken);
        if (snippet == null) return false;

        snippet.CategoryId = dto.CategoryId;
        snippet.LanguageKey = dto.LanguageKey.Trim().ToLower();
        snippet.DisplayName = dto.DisplayName.Trim();
        snippet.FileName = dto.FileName.Trim();
        snippet.BindingTag = dto.BindingTag.Trim();
        snippet.PackageTag = dto.PackageTag.Trim();
        snippet.MemoryTag = dto.MemoryTag.Trim();
        snippet.ThreadsTag = dto.ThreadsTag.Trim();
        snippet.SortOrder = dto.SortOrder;
        snippet.CodeContent = dto.CodeContent;
        snippet.IsActive = dto.IsActive;
        snippet.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteSdkSnippetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var snippet = await _context.SdkCodeSnippets.FindAsync(new object[] { id }, cancellationToken);
        if (snippet == null) return false;

        _context.SdkCodeSnippets.Remove(snippet);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // 2. Inquiries CRM
    public async Task<List<ContactInquiry>> GetInquiriesAsync(string? status = null, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = _context.ContactInquiries.AsNoTracking().Include(i => i.Notes).AsQueryable();

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<InquiryStatus>(status, true, out var st))
        {
            query = query.Where(i => i.Status == st);
        }

        return await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<ContactInquiry?> GetInquiryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ContactInquiries
            .AsNoTracking()
            .Include(i => i.Notes)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }

    public async Task<bool> UpdateInquiryStatusAsync(Guid id, UpdateInquiryStatusDto dto, CancellationToken cancellationToken = default)
    {
        var inquiry = await _context.ContactInquiries.FindAsync(new object[] { id }, cancellationToken);
        if (inquiry == null) return false;

        if (Enum.TryParse<InquiryStatus>(dto.Status, true, out var st))
        {
            inquiry.Status = st;
        }

        if (!string.IsNullOrWhiteSpace(dto.Priority) && Enum.TryParse<InquiryPriority>(dto.Priority, true, out var pr))
        {
            inquiry.Priority = pr;
        }

        if (dto.AssignedAdminName != null)
        {
            inquiry.AssignedAdminName = dto.AssignedAdminName;
        }

        inquiry.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<InquiryNote?> AddInquiryNoteAsync(Guid id, AddInquiryNoteDto dto, CancellationToken cancellationToken = default)
    {
        var inquiry = await _context.ContactInquiries.FindAsync(new object[] { id }, cancellationToken);
        if (inquiry == null) return null;

        var note = new InquiryNote
        {
            InquiryId = inquiry.Id,
            AuthorName = dto.AuthorName.Trim(),
            Content = dto.Content.Trim()
        };

        await _context.InquiryNotes.AddAsync(note, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return note;
    }

    public async Task<bool> DeleteInquiryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var inquiry = await _context.ContactInquiries.FindAsync(new object[] { id }, cancellationToken);
        if (inquiry == null) return false;

        _context.ContactInquiries.Remove(inquiry);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // 3. Careers & ATS
    public async Task<JobPosting> CreateJobPostingAsync(UpsertJobPostingDto dto, CancellationToken cancellationToken = default)
    {
        Enum.TryParse<LocationType>(dto.LocationType, true, out var loc);
        Enum.TryParse<EmploymentType>(dto.EmploymentType, true, out var emp);
        Enum.TryParse<ExperienceLevel>(dto.ExperienceLevel, true, out var exp);
        Enum.TryParse<JobStatus>(dto.Status, true, out var stat);

        var job = new JobPosting
        {
            DepartmentId = dto.DepartmentId,
            Title = dto.Title.Trim(),
            Slug = dto.Slug.Trim().ToLower(),
            LocationType = loc,
            EmploymentType = emp,
            ExperienceLevel = exp,
            Description = dto.Description.Trim(),
            Requirements = dto.Requirements.Trim(),
            TechStackTags = dto.TechStackTags.Trim(),
            Status = stat,
            SortOrder = dto.SortOrder
        };

        await _context.JobPostings.AddAsync(job, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return job;
    }

    public async Task<bool> UpdateJobPostingAsync(Guid id, UpsertJobPostingDto dto, CancellationToken cancellationToken = default)
    {
        var job = await _context.JobPostings.FindAsync(new object[] { id }, cancellationToken);
        if (job == null) return false;

        Enum.TryParse<LocationType>(dto.LocationType, true, out var loc);
        Enum.TryParse<EmploymentType>(dto.EmploymentType, true, out var emp);
        Enum.TryParse<ExperienceLevel>(dto.ExperienceLevel, true, out var exp);
        Enum.TryParse<JobStatus>(dto.Status, true, out var stat);

        job.DepartmentId = dto.DepartmentId;
        job.Title = dto.Title.Trim();
        job.Slug = dto.Slug.Trim().ToLower();
        job.LocationType = loc;
        job.EmploymentType = emp;
        job.ExperienceLevel = exp;
        job.Description = dto.Description.Trim();
        job.Requirements = dto.Requirements.Trim();
        job.TechStackTags = dto.TechStackTags.Trim();
        job.Status = stat;
        job.SortOrder = dto.SortOrder;
        job.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteJobPostingAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var job = await _context.JobPostings.FindAsync(new object[] { id }, cancellationToken);
        if (job == null) return false;

        _context.JobPostings.Remove(job);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<List<JobApplication>> GetJobApplicationsAsync(string? stage = null, int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = _context.JobApplications
            .AsNoTracking()
            .Include(a => a.Candidate)
            .Include(a => a.Job)
            .Include(a => a.Notes)
            .Include(a => a.Interviews)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(stage) && Enum.TryParse<ApplicationStage>(stage, true, out var stg))
        {
            query = query.Where(a => a.Status == stg);
        }

        return await query
            .OrderByDescending(a => a.AppliedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<JobApplication?> GetJobApplicationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.JobApplications
            .AsNoTracking()
            .Include(a => a.Candidate)
            .Include(a => a.Job)
            .Include(a => a.Notes)
            .Include(a => a.Interviews)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<bool> UpdateApplicationStageAsync(Guid id, UpdateApplicationStageDto dto, CancellationToken cancellationToken = default)
    {
        var app = await _context.JobApplications.FindAsync(new object[] { id }, cancellationToken);
        if (app == null) return false;

        if (Enum.TryParse<ApplicationStage>(dto.Stage, true, out var stg))
        {
            app.Status = stg;
        }

        if (dto.Rating.HasValue)
        {
            app.Rating = Math.Clamp(dto.Rating.Value, 0, 5);
        }

        if (!string.IsNullOrWhiteSpace(dto.RejectionReason))
        {
            app.RejectionReason = dto.RejectionReason;
        }

        app.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ApplicationNote?> AddApplicationNoteAsync(Guid id, AddApplicationNoteDto dto, CancellationToken cancellationToken = default)
    {
        var app = await _context.JobApplications.FindAsync(new object[] { id }, cancellationToken);
        if (app == null) return null;

        var note = new ApplicationNote
        {
            ApplicationId = app.Id,
            AuthorName = dto.AuthorName.Trim(),
            Content = dto.Content.Trim(),
            Rating = dto.Rating
        };

        await _context.ApplicationNotes.AddAsync(note, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return note;
    }

    public async Task<ApplicationInterview?> ScheduleInterviewAsync(Guid id, ScheduleInterviewDto dto, CancellationToken cancellationToken = default)
    {
        var app = await _context.JobApplications.FindAsync(new object[] { id }, cancellationToken);
        if (app == null) return null;

        var interview = new ApplicationInterview
        {
            ApplicationId = app.Id,
            InterviewerName = dto.InterviewerName.Trim(),
            StageName = dto.StageName.Trim(),
            ScheduledAt = dto.ScheduledAt,
            MeetingLink = dto.MeetingLink?.Trim(),
            Outcome = "Scheduled"
        };

        await _context.ApplicationInterviews.AddAsync(interview, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return interview;
    }

    // 4. Systems Academy
    public async Task<AcademyCohort> CreateCohortAsync(UpsertCohortDto dto, CancellationToken cancellationToken = default)
    {
        Enum.TryParse<CohortStatus>(dto.Status, true, out var st);
        var cohort = new AcademyCohort
        {
            TrackId = dto.TrackId,
            Name = dto.Name.Trim(),
            ApplicationDeadline = dto.ApplicationDeadline,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            MaxSeats = dto.MaxSeats,
            Status = st
        };

        await _context.AcademyCohorts.AddAsync(cohort, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cohort;
    }

    public async Task<bool> UpdateCohortAsync(Guid id, UpsertCohortDto dto, CancellationToken cancellationToken = default)
    {
        var cohort = await _context.AcademyCohorts.FindAsync(new object[] { id }, cancellationToken);
        if (cohort == null) return false;

        Enum.TryParse<CohortStatus>(dto.Status, true, out var st);
        cohort.TrackId = dto.TrackId;
        cohort.Name = dto.Name.Trim();
        cohort.ApplicationDeadline = dto.ApplicationDeadline;
        cohort.StartDate = dto.StartDate;
        cohort.EndDate = dto.EndDate;
        cohort.MaxSeats = dto.MaxSeats;
        cohort.Status = st;
        cohort.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteCohortAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cohort = await _context.AcademyCohorts.FindAsync(new object[] { id }, cancellationToken);
        if (cohort == null) return false;

        _context.AcademyCohorts.Remove(cohort);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<List<StudentApplication>> GetStudentApplicationsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.StudentApplications
            .AsNoTracking()
            .Include(a => a.Cohort)
                .ThenInclude(c => c!.Track)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateStudentAppStatusAsync(Guid id, UpdateStudentAppStatusDto dto, CancellationToken cancellationToken = default)
    {
        var app = await _context.StudentApplications.FindAsync(new object[] { id }, cancellationToken);
        if (app == null) return false;

        if (Enum.TryParse<StudentApplicationStatus>(dto.Status, true, out var st))
        {
            app.Status = st;
        }

        app.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<CertificateIssuedResponseDto> IssueCertificateAsync(IssueCertificateDto dto, CancellationToken cancellationToken = default)
    {
        var enrollment = await _context.StudentEnrollments.FindAsync(new object[] { dto.EnrollmentId }, cancellationToken);
        if (enrollment == null)
        {
            throw new InvalidOperationException("Student enrollment not found.");
        }

        var randomHex = Convert.ToHexString(RandomNumberGenerator.GetBytes(4));
        var certCode = $"6174-CERT-{randomHex}";
        var hashInput = $"{certCode}:{dto.RecipientName}:{dto.TrackName}:{DateTime.UtcNow:O}";
        var sha256 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(hashInput)));

        var cert = new Certification
        {
            EnrollmentId = enrollment.Id,
            CertCode = certCode,
            RecipientName = dto.RecipientName.Trim(),
            TrackName = dto.TrackName.Trim(),
            Sha256Hash = sha256,
            SignedByAdminName = dto.SignedByAdminName.Trim(),
            IssuedAt = DateTime.UtcNow,
            VerificationUrl = $"/verify.html?code={certCode}"
        };

        enrollment.IsGraduated = true;
        enrollment.UpdatedAt = DateTime.UtcNow;

        await _context.Certifications.AddAsync(cert, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CertificateIssuedResponseDto
        {
            CertificateId = cert.Id,
            CertCode = cert.CertCode,
            RecipientName = cert.RecipientName,
            TrackName = cert.TrackName,
            Sha256Hash = cert.Sha256Hash,
            VerificationUrl = cert.VerificationUrl,
            IssuedAt = cert.IssuedAt
        };
    }

    public async Task<List<Certification>> GetCertificatesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Certifications
            .AsNoTracking()
            .Include(c => c.Enrollment)
            .OrderByDescending(c => c.IssuedAt)
            .ToListAsync(cancellationToken);
    }

    // 5. Telemetry & Maintenance
    public async Task<List<TelemetryNode>> GetTelemetryNodesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TelemetryNodes
            .AsNoTracking()
            .Include(n => n.Snapshots)
            .OrderBy(n => n.NodeIdentifier)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateNodeStatusAsync(Guid id, ClusterStatus status, CancellationToken cancellationToken = default)
    {
        var node = await _context.TelemetryNodes.FindAsync(new object[] { id }, cancellationToken);
        if (node == null) return false;

        node.Status = status;
        node.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<MaintenanceAnnouncement> CreateAnnouncementAsync(UpsertAnnouncementDto dto, CancellationToken cancellationToken = default)
    {
        Enum.TryParse<SeverityLevel>(dto.Severity, true, out var sev);
        var ann = new MaintenanceAnnouncement
        {
            Title = dto.Title.Trim(),
            Message = dto.Message.Trim(),
            Severity = sev,
            IsActive = dto.IsActive,
            StartsAt = dto.StartsAt,
            EndsAt = dto.EndsAt
        };

        await _context.MaintenanceAnnouncements.AddAsync(ann, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return ann;
    }

    public async Task<bool> UpdateAnnouncementAsync(Guid id, UpsertAnnouncementDto dto, CancellationToken cancellationToken = default)
    {
        var ann = await _context.MaintenanceAnnouncements.FindAsync(new object[] { id }, cancellationToken);
        if (ann == null) return false;

        Enum.TryParse<SeverityLevel>(dto.Severity, true, out var sev);
        ann.Title = dto.Title.Trim();
        ann.Message = dto.Message.Trim();
        ann.Severity = sev;
        ann.IsActive = dto.IsActive;
        ann.StartsAt = dto.StartsAt;
        ann.EndsAt = dto.EndsAt;
        ann.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAnnouncementAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ann = await _context.MaintenanceAnnouncements.FindAsync(new object[] { id }, cancellationToken);
        if (ann == null) return false;

        _context.MaintenanceAnnouncements.Remove(ann);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // 6. Forensic Audit Trail
    public async Task<List<AuditLog>> GetAuditLogsAsync(int page = 1, int pageSize = 50, CancellationToken cancellationToken = default)
    {
        return await _context.AuditLogs
            .AsNoTracking()
            .Include(a => a.User)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task LogActionAsync(Guid? userId, string action, string entityType, Guid? entityId, string? diffJson = null, string? clientIp = null, CancellationToken cancellationToken = default)
    {
        var log = new AuditLog
        {
            UserId = userId,
            Action = action.Trim().ToUpper(),
            EntityType = entityType.Trim().ToUpper(),
            EntityId = entityId,
            DiffPayload = diffJson,
            IpAddress = clientIp
        };

        await _context.AuditLogs.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
