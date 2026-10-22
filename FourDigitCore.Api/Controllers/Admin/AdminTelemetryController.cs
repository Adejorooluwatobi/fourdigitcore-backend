using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Application.Services;
using FourDigitCore.Domain.Entities;
using FourDigitCore.Domain.Enums;

namespace FourDigitCore.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/telemetry")]
[Route("api/v1/admin/telemetry")]
[Authorize]
public class AdminTelemetryController : ControllerBase
{
    private readonly IFourDigitCoreAdminService _adminService;

    public AdminTelemetryController(IFourDigitCoreAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("nodes")]
    [ProducesResponseType(typeof(List<TelemetryNode>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNodes(CancellationToken cancellationToken)
    {
        var nodes = await _adminService.GetTelemetryNodesAsync(cancellationToken);
        return Ok(nodes);
    }

    [HttpPut("nodes/{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateNodeStatus(Guid id, [FromQuery] ClusterStatus status, CancellationToken cancellationToken)
    {
        var success = await _adminService.UpdateNodeStatusAsync(id, status, cancellationToken);
        if (!success) return NotFound(new { message = "Node not found." });

        return Ok(new { message = $"Node status updated to {status}." });
    }

    [HttpGet("announcements")]
    [ProducesResponseType(typeof(List<MaintenanceAnnouncement>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAnnouncements(CancellationToken cancellationToken)
    {
        var announcements = await _adminService.GetAnnouncementsAsync(cancellationToken);
        return Ok(announcements);
    }

    [HttpPost("announcements")]
    [ProducesResponseType(typeof(MaintenanceAnnouncement), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAnnouncement([FromBody] UpsertAnnouncementDto dto, CancellationToken cancellationToken)
    {
        var ann = await _adminService.CreateAnnouncementAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(CreateAnnouncement), new { id = ann.Id }, ann);
    }

    [HttpPut("announcements/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAnnouncement(Guid id, [FromBody] UpsertAnnouncementDto dto, CancellationToken cancellationToken)
    {
        var success = await _adminService.UpdateAnnouncementAsync(id, dto, cancellationToken);
        if (!success) return NotFound(new { message = "Announcement not found." });

        return Ok(new { message = "Announcement updated successfully." });
    }

    [HttpDelete("announcements/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAnnouncement(Guid id, CancellationToken cancellationToken)
    {
        var success = await _adminService.DeleteAnnouncementAsync(id, cancellationToken);
        if (!success) return NotFound(new { message = "Announcement not found." });

        return Ok(new { message = "Announcement deleted successfully." });
    }
}
