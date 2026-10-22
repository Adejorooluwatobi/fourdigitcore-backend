using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Application.Services;
using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/careers")]
[Route("api/v1/admin/careers")]
[Authorize]
public class AdminCareersController : ControllerBase
{
    private readonly IFourDigitCoreAdminService _adminService;

    public AdminCareersController(IFourDigitCoreAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("jobs")]
    [ProducesResponseType(typeof(List<JobPosting>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobs([FromServices] IAppDbContext context, CancellationToken cancellationToken)
    {
        var jobs = await context.JobPostings
            .AsNoTracking()
            .OrderBy(j => j.SortOrder)
            .ToListAsync(cancellationToken);
        return Ok(jobs);
    }

    [HttpPost("jobs")]
    [ProducesResponseType(typeof(JobPosting), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJob([FromBody] UpsertJobPostingDto dto, CancellationToken cancellationToken)
    {
        var job = await _adminService.CreateJobPostingAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(CreateJob), new { id = job.Id }, job);
    }

    [HttpPut("jobs/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpsertJobPostingDto dto, CancellationToken cancellationToken)
    {
        var success = await _adminService.UpdateJobPostingAsync(id, dto, cancellationToken);
        if (!success) return NotFound(new { message = "Job posting not found." });

        return Ok(new { message = "Job posting updated successfully." });
    }

    [HttpDelete("jobs/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJob(Guid id, CancellationToken cancellationToken)
    {
        var success = await _adminService.DeleteJobPostingAsync(id, cancellationToken);
        if (!success) return NotFound(new { message = "Job posting not found." });

        return Ok(new { message = "Job posting deleted successfully." });
    }

    [HttpGet("applications")]
    [ProducesResponseType(typeof(List<JobApplication>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetApplications([FromQuery] string? stage, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var apps = await _adminService.GetJobApplicationsAsync(stage, page, pageSize, cancellationToken);
        return Ok(apps);
    }

    [HttpGet("applications/{id:guid}")]
    [ProducesResponseType(typeof(JobApplication), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetApplicationById(Guid id, CancellationToken cancellationToken)
    {
        var app = await _adminService.GetJobApplicationByIdAsync(id, cancellationToken);
        if (app == null) return NotFound(new { message = "Job application not found." });

        return Ok(app);
    }

    [HttpPut("applications/{id:guid}/stage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStage(Guid id, [FromBody] UpdateApplicationStageDto dto, CancellationToken cancellationToken)
    {
        var success = await _adminService.UpdateApplicationStageAsync(id, dto, cancellationToken);
        if (!success) return NotFound(new { message = "Job application not found." });

        return Ok(new { message = "Application stage updated successfully." });
    }

    [HttpPost("applications/{id:guid}/notes")]
    [ProducesResponseType(typeof(ApplicationNote), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddNote(Guid id, [FromBody] AddApplicationNoteDto dto, CancellationToken cancellationToken)
    {
        var note = await _adminService.AddApplicationNoteAsync(id, dto, cancellationToken);
        if (note == null) return NotFound(new { message = "Job application not found." });

        return Ok(note);
    }

    [HttpPost("applications/{id:guid}/interviews")]
    [ProducesResponseType(typeof(ApplicationInterview), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ScheduleInterview(Guid id, [FromBody] ScheduleInterviewDto dto, CancellationToken cancellationToken)
    {
        var interview = await _adminService.ScheduleInterviewAsync(id, dto, cancellationToken);
        if (interview == null) return NotFound(new { message = "Job application not found." });

        return Ok(interview);
    }
}
