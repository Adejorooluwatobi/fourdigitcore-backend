using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/site-settings")]
[AllowAnonymous]
public class SiteSettingsController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public SiteSettingsController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SiteSettingsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSiteSettings(CancellationToken cancellationToken)
    {
        var settings = await _publicService.GetSiteSettingsAsync(cancellationToken);
        return Ok(settings);
    }
}
