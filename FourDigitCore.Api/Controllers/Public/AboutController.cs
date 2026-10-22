using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/about")]
[AllowAnonymous]
public class AboutController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public AboutController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(AboutDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAbout(CancellationToken cancellationToken)
    {
        var about = await _publicService.GetAboutAsync(cancellationToken);
        return Ok(about);
    }
}
