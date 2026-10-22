using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Application.DTOs.Auth;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/auth")]
[Route("api/v1/admin/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAdminUserManagementService _adminUserManagementService;

    public AuthController(
        IAuthService authService,
        IAdminUserManagementService adminUserManagementService)
    {
        _authService = authService;
        _adminUserManagementService = adminUserManagementService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers.UserAgent.ToString();

        var response = await _authService.LoginAsync(request, ip, userAgent, cancellationToken);
        if (response == null)
            return Unauthorized(new { message = "Invalid email or credentials." });

        return Ok(response);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request, CancellationToken cancellationToken)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers.UserAgent.ToString();

        var response = await _authService.RefreshTokenAsync(request, ip, userAgent, cancellationToken);
        if (response == null)
            return Unauthorized(new { message = "Invalid or expired refresh token." });

        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(AdminUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var subClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(subClaim) || !Guid.TryParse(subClaim, out var userId))
            return Unauthorized();

        var user = await _authService.GetCurrentUserAsync(userId, cancellationToken);
        if (user == null)
            return Unauthorized();

        return Ok(user);
    }

    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto request, CancellationToken cancellationToken)
    {
        var subClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(subClaim) || !Guid.TryParse(subClaim, out var userId))
            return Unauthorized();

        var success = await _authService.ChangePasswordAsync(userId, request, cancellationToken);
        if (!success)
            return BadRequest(new { message = "Current password verification failed." });

        return Ok(new { message = "Password updated successfully." });
    }

    // Public Onboarding: Check Invitation Info
    [HttpGet("invitation-info")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(InvitationInfoResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInvitationInfo([FromQuery] string token, CancellationToken cancellationToken)
    {
        var info = await _adminUserManagementService.GetInvitationInfoAsync(token, cancellationToken);
        return Ok(info);
    }

    // Public Onboarding: Accept Invitation & Set Password
    [HttpPost("accept-invitation")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationRequestDto dto, CancellationToken cancellationToken)
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

        try
        {
            var response = await _adminUserManagementService.AcceptInvitationAsync(dto, clientIp, cancellationToken);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
