using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/home")]
[AllowAnonymous]
public class HomeController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public HomeController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(HomePlatformDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHomePlatform(CancellationToken cancellationToken)
    {
        var platform = await _publicService.GetHomePlatformAsync(cancellationToken);
        return Ok(platform);
    }
}
