using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/solutions")]
[AllowAnonymous]
public class SolutionsController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public SolutionsController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SolutionsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSolutions(CancellationToken cancellationToken)
    {
        var solutions = await _publicService.GetSolutionsAsync(cancellationToken);
        return Ok(solutions);
    }
}
