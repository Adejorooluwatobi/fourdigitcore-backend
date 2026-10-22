using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Application.Services;
using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Api.Controllers.Admin;

[ApiController]
[Route("api/v1/admin/sdk")]
[Authorize]
public class AdminSdkController : ControllerBase
{
    private readonly IFourDigitCoreAdminService _adminService;

    public AdminSdkController(IFourDigitCoreAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPut("section")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSection([FromBody] UpdateSdkSectionDto dto, CancellationToken cancellationToken)
    {
        await _adminService.UpdateSdkSectionAsync(dto, cancellationToken);
        return Ok(new { message = "SDK showcase section updated successfully." });
    }

    [HttpPost("categories")]
    [ProducesResponseType(typeof(SdkCategory), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCategory([FromBody] UpsertSdkCategoryDto dto, CancellationToken cancellationToken)
    {
        var category = await _adminService.CreateSdkCategoryAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(CreateCategory), new { id = category.Id }, category);
    }

    [HttpPut("categories/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpsertSdkCategoryDto dto, CancellationToken cancellationToken)
    {
        var success = await _adminService.UpdateSdkCategoryAsync(id, dto, cancellationToken);
        if (!success) return NotFound(new { message = "SDK category not found." });

        return Ok(new { message = "Category updated successfully." });
    }

    [HttpDelete("categories/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        var success = await _adminService.DeleteSdkCategoryAsync(id, cancellationToken);
        if (!success) return NotFound(new { message = "SDK category not found." });

        return Ok(new { message = "Category deleted successfully." });
    }

    [HttpPost("snippets")]
    [ProducesResponseType(typeof(SdkCodeSnippet), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSnippet([FromBody] UpsertSdkSnippetDto dto, CancellationToken cancellationToken)
    {
        var snippet = await _adminService.CreateSdkSnippetAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(CreateSnippet), new { id = snippet.Id }, snippet);
    }

    [HttpPut("snippets/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSnippet(Guid id, [FromBody] UpsertSdkSnippetDto dto, CancellationToken cancellationToken)
    {
        var success = await _adminService.UpdateSdkSnippetAsync(id, dto, cancellationToken);
        if (!success) return NotFound(new { message = "SDK snippet not found." });

        return Ok(new { message = "SDK snippet updated successfully." });
    }

    [HttpDelete("snippets/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSnippet(Guid id, CancellationToken cancellationToken)
    {
        var success = await _adminService.DeleteSdkSnippetAsync(id, cancellationToken);
        if (!success) return NotFound(new { message = "SDK snippet not found." });

        return Ok(new { message = "SDK snippet deleted successfully." });
    }
}
