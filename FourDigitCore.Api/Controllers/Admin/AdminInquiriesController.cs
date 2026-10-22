using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Application.Services;
using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/inquiries")]
[Route("api/v1/admin/inquiries")]
[Authorize]
public class AdminInquiriesController : ControllerBase
{
    private readonly IFourDigitCoreAdminService _adminService;

    public AdminInquiriesController(IFourDigitCoreAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ContactInquiry>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInquiries([FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var inquiries = await _adminService.GetInquiriesAsync(status, page, pageSize, cancellationToken);
        return Ok(inquiries);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContactInquiry), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInquiryById(Guid id, CancellationToken cancellationToken)
    {
        var inquiry = await _adminService.GetInquiryByIdAsync(id, cancellationToken);
        if (inquiry == null) return NotFound(new { message = "Inquiry not found." });

        return Ok(inquiry);
    }

    [HttpPut("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateInquiryStatusDto dto, CancellationToken cancellationToken)
    {
        var success = await _adminService.UpdateInquiryStatusAsync(id, dto, cancellationToken);
        if (!success) return NotFound(new { message = "Inquiry not found." });

        return Ok(new { message = "Inquiry status updated successfully." });
    }

    [HttpPost("{id:guid}/notes")]
    [ProducesResponseType(typeof(InquiryNote), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddNote(Guid id, [FromBody] AddInquiryNoteDto dto, CancellationToken cancellationToken)
    {
        var note = await _adminService.AddInquiryNoteAsync(id, dto, cancellationToken);
        if (note == null) return NotFound(new { message = "Inquiry not found." });

        return CreatedAtAction(nameof(GetInquiryById), new { id }, note);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInquiry(Guid id, CancellationToken cancellationToken)
    {
        var success = await _adminService.DeleteInquiryAsync(id, cancellationToken);
        if (!success) return NotFound(new { message = "Inquiry not found." });

        return Ok(new { message = "Inquiry deleted successfully." });
    }
}
