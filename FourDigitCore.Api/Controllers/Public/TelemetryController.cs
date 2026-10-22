using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/telemetry")]
[AllowAnonymous]
public class TelemetryController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public TelemetryController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet("status")]
    [ProducesResponseType(typeof(TelemetryStatusDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTelemetryStatus(CancellationToken cancellationToken)
    {
        var status = await _publicService.GetTelemetryStatusAsync(cancellationToken);
        return Ok(status);
    }
}
