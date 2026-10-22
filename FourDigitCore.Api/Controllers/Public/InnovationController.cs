using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/innovation")]
[AllowAnonymous]
public class InnovationController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public InnovationController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(InnovationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInnovation(CancellationToken cancellationToken)
    {
        var innovation = await _publicService.GetInnovationAsync(cancellationToken);
        return Ok(innovation);
    }

    [HttpGet("whitepapers")]
    [ProducesResponseType(typeof(List<WhitepaperDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWhitepapers(CancellationToken cancellationToken)
    {
        var whitepapers = await _publicService.GetWhitepapersAsync(cancellationToken);
        return Ok(whitepapers);
    }

    [HttpPost("whitepapers/{id:guid}/download")]
    [ProducesResponseType(typeof(WhitepaperDownloadResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadWhitepaper(Guid id, [FromBody] WhitepaperDownloadRequestDto request, CancellationToken cancellationToken)
    {
        request.WhitepaperId = id;
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        try
        {
            var response = await _publicService.SubmitWhitepaperDownloadAsync(request, clientIp, cancellationToken);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
