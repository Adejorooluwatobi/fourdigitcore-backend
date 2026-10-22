using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Public;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Api.Controllers.Public;

[ApiController]
[Route("api/v1/careers")]
[AllowAnonymous]
public class CareersController : ControllerBase
{
    private readonly IFourDigitCorePublicService _publicService;

    public CareersController(IFourDigitCorePublicService publicService)
    {
        _publicService = publicService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(CareersDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCareers(CancellationToken cancellationToken)
    {
        var careers = await _publicService.GetCareersAsync(cancellationToken);
        return Ok(careers);
    }

    [HttpGet("{slugOrId}")]
    [ProducesResponseType(typeof(JobPostingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobDetail(string slugOrId, CancellationToken cancellationToken)
    {
        var job = await _publicService.GetJobPostingBySlugOrIdAsync(slugOrId, cancellationToken);
        if (job == null)
            return NotFound(new { message = $"Job posting '{slugOrId}' not found." });

        return Ok(job);
    }

    [HttpPost("apply")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(JobApplicationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApplyForJob([FromForm] JobApplicationRequestDto request, IFormFile? resume, CancellationToken cancellationToken)
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
        try
        {
            Stream? stream = null;
            string? fileName = null;
            string? contentType = null;

            if (resume != null && resume.Length > 0)
            {
                stream = resume.OpenReadStream();
                fileName = resume.FileName;
                contentType = resume.ContentType;
            }

            var response = await _publicService.SubmitJobApplicationAsync(request, stream, fileName, contentType, clientIp, cancellationToken);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
