using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/inquiries")]
[AllowAnonymous]
public class InquiriesController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public InquiriesController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(InquiryResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SubmitInquiry([FromBody] CreateInquiryRequestDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { message = "Full name and email are mandatory." });
        }

        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        var response = await _publicService.SubmitContactInquiryAsync(request, clientIp, cancellationToken);
        return Ok(response);
    }
}
