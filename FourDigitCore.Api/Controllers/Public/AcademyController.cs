using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/academy")]
[AllowAnonymous]
public class AcademyController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public AcademyController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(AcademyDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAcademy(CancellationToken cancellationToken)
    {
        var academy = await _publicService.GetAcademyAsync(cancellationToken);
        return Ok(academy);
    }

    [HttpGet("cohorts")]
    [ProducesResponseType(typeof(List<AcademyCohortDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCohorts(CancellationToken cancellationToken)
    {
        var cohorts = await _publicService.GetAcademyCohortsAsync(cancellationToken);
        return Ok(cohorts);
    }

    [HttpPost("apply")]
    [ProducesResponseType(typeof(StudentApplicationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApplyToCohort([FromBody] StudentApplicationRequestDto request, CancellationToken cancellationToken)
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        try
        {
            var response = await _publicService.SubmitStudentApplicationAsync(request, clientIp, cancellationToken);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
