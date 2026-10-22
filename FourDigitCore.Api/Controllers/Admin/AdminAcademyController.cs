using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Application.Services;
using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Api.Controllers.Admin;

[ApiController]
[Route("api/v1/admin/academy")]
[Authorize]
public class AdminAcademyController : ControllerBase
{
    private readonly IFourDigitCoreAdminService _adminService;

    public AdminAcademyController(IFourDigitCoreAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("cohorts")]
    [ProducesResponseType(typeof(AcademyCohort), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCohort([FromBody] UpsertCohortDto dto, CancellationToken cancellationToken)
    {
        var cohort = await _adminService.CreateCohortAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(CreateCohort), new { id = cohort.Id }, cohort);
    }

    [HttpPut("cohorts/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCohort(Guid id, [FromBody] UpsertCohortDto dto, CancellationToken cancellationToken)
    {
        var success = await _adminService.UpdateCohortAsync(id, dto, cancellationToken);
        if (!success) return NotFound(new { message = "Cohort not found." });

        return Ok(new { message = "Cohort updated successfully." });
    }

    [HttpDelete("cohorts/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCohort(Guid id, CancellationToken cancellationToken)
    {
        var success = await _adminService.DeleteCohortAsync(id, cancellationToken);
        if (!success) return NotFound(new { message = "Cohort not found." });

        return Ok(new { message = "Cohort deleted successfully." });
    }

    [HttpGet("applications")]
    [ProducesResponseType(typeof(List<StudentApplication>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentApplications(CancellationToken cancellationToken)
    {
        var apps = await _adminService.GetStudentApplicationsAsync(cancellationToken);
        return Ok(apps);
    }

    [HttpPut("applications/{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStudentAppStatus(Guid id, [FromBody] UpdateStudentAppStatusDto dto, CancellationToken cancellationToken)
    {
        var success = await _adminService.UpdateStudentAppStatusAsync(id, dto, cancellationToken);
        if (!success) return NotFound(new { message = "Student application not found." });

        return Ok(new { message = "Student application status updated." });
    }

    [HttpPost("certificates/issue")]
    [ProducesResponseType(typeof(CertificateIssuedResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> IssueCertificate([FromBody] IssueCertificateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _adminService.IssueCertificateAsync(dto, cancellationToken);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("certificates")]
    [ProducesResponseType(typeof(List<Certification>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCertificates(CancellationToken cancellationToken)
    {
        var certs = await _adminService.GetCertificatesAsync(cancellationToken);
        return Ok(certs);
    }
}
