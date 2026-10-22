using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/navigation")]
[AllowAnonymous]
public class NavigationController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public NavigationController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(NavigationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNavigation(CancellationToken cancellationToken)
    {
        var nav = await _publicService.GetNavigationAsync(cancellationToken);
        return Ok(nav);
    }
}
