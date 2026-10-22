using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/certificates")]
[AllowAnonymous]
public class CertificatesController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public CertificatesController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet("verify/{code}")]
    [ProducesResponseType(typeof(CertificateVerificationResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerifyCertificate(string code, CancellationToken cancellationToken)
    {
        var response = await _publicService.VerifyCertificateAsync(code, cancellationToken);
        return Ok(response);
    }
}
