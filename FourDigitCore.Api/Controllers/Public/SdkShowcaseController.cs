using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/sdk-showcase")]
[AllowAnonymous]
public class SdkShowcaseController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public SdkShowcaseController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SdkShowcaseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSdkShowcase(CancellationToken cancellationToken)
    {
        var showcase = await _publicService.GetSdkShowcaseAsync(cancellationToken);
        return Ok(showcase);
    }

    [HttpGet("{languageKey}")]
    [ProducesResponseType(typeof(SdkSnippetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSnippetByLanguage(string languageKey, CancellationToken cancellationToken)
    {
        var snippet = await _publicService.GetSdkSnippetByLanguageAsync(languageKey, cancellationToken);
        if (snippet == null)
            return NotFound(new { message = $"No SDK snippet found for language: {languageKey}" });

        return Ok(snippet);
    }
}
