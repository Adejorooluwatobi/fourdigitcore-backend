using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Admin;

namespace FourDigitCore.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/users")]
[Route("api/v1/admin/users")]
[Authorize(Roles = "SUPERADMIN")]
[Produces("application/json")]
public class AdminUsersController : ControllerBase
{
    private readonly IAdminUserManagementService _adminService;
    private readonly IFileStorageService _fileStorageService;

    public AdminUsersController(
        IAdminUserManagementService adminService,
        IFileStorageService fileStorageService)
    {
        _adminService = adminService;
        _fileStorageService = fileStorageService;
    }

    // 1. List All Admins
    [HttpGet]
    [ProducesResponseType(typeof(List<SystemAdminDetailDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAdmins(CancellationToken cancellationToken)
    {
        var admins = await _adminService.GetAllAdminsAsync(cancellationToken);
        return Ok(admins);
    }

    // 2. Invite New Admin
    [HttpPost("invite")]
    [ProducesResponseType(typeof(AdminInvitationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InviteAdmin([FromBody] InviteAdminDto dto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.FullName))
        {
            return BadRequest(new { message = "Email and Full Name are required." });
        }

        var currentAdminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var currentAdminId = Guid.TryParse(currentAdminIdStr, out var id) ? id : Guid.Empty;
        var currentAdminName = User.FindFirstValue(ClaimTypes.Name) ?? "System SuperAdmin";

        try
        {
            var invitation = await _adminService.InviteAdminAsync(dto, currentAdminId, currentAdminName, cancellationToken);
            return Ok(invitation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 3. View Invitations
    [HttpGet("invitations")]
    [ProducesResponseType(typeof(List<AdminInvitationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInvitations(CancellationToken cancellationToken)
    {
        var invitations = await _adminService.GetInvitationsAsync(cancellationToken);
        return Ok(invitations);
    }

    // 4. Resend Invitation
    [HttpPost("invitations/{id:guid}/resend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResendInvitation([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _adminService.ResendInvitationAsync(id, cancellationToken);
            if (!success) return NotFound(new { message = "Invitation not found." });
            return Ok(new { message = "Invitation resent successfully with updated expiration." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 5. Revoke Invitation
    [HttpDelete("invitations/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RevokeInvitation([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var success = await _adminService.RevokeInvitationAsync(id, cancellationToken);
        if (!success) return NotFound(new { message = "Invitation not found." });
        return Ok(new { message = "Invitation revoked." });
    }

    // 6. Suspend Admin (Force Session Revocation)
    [HttpPut("{id:guid}/suspend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SuspendAdmin([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentAdminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var currentAdminId = Guid.TryParse(currentAdminIdStr, out var adminId) ? adminId : Guid.Empty;

        try
        {
            var success = await _adminService.SuspendAdminAsync(id, currentAdminId, cancellationToken);
            if (!success) return NotFound(new { message = "Administrator account not found." });
            return Ok(new { message = "Administrator account has been suspended and all active sessions revoked." });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }

    // 7. Reactivate Admin
    [HttpPut("{id:guid}/reactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReactivateAdmin([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentAdminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var currentAdminId = Guid.TryParse(currentAdminIdStr, out var adminId) ? adminId : Guid.Empty;

        var success = await _adminService.ReactivateAdminAsync(id, currentAdminId, cancellationToken);
        if (!success) return NotFound(new { message = "Administrator account not found." });
        return Ok(new { message = "Administrator account reactivated successfully." });
    }

    // 8. Delete Admin
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAdmin([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var currentAdminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        var currentAdminId = Guid.TryParse(currentAdminIdStr, out var adminId) ? adminId : Guid.Empty;

        try
        {
            var success = await _adminService.DeleteAdminAsync(id, currentAdminId, cancellationToken);
            if (!success) return NotFound(new { message = "Administrator account not found." });
            return Ok(new { message = "Administrator account deleted successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }

    // 9. Media & Asset Upload (Cloudinary CDN with Local Fallback)
    [HttpPost("upload")]
    [ProducesResponseType(typeof(UploadMediaResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadMedia([FromForm] IFormFile file, [FromQuery] string folder = "fourdigitcore", CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "File cannot be empty." });
        }

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var isImage = ext is ".jpg" or ".jpeg" or ".png" or ".webp" or ".gif" or ".svg";

        using var stream = file.OpenReadStream();
        UploadMediaResultDto result;

        if (isImage)
        {
            result = await _fileStorageService.UploadImageAsync(stream, file.FileName, folder, cancellationToken);
        }
        else
        {
            result = await _fileStorageService.UploadRawFileAsync(stream, file.FileName, folder, cancellationToken);
        }

        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(result);
    }
}
