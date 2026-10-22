using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FourDigitCore.Application.Services;
using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Api.Controllers.Admin;

[ApiController]
[Route("api/v1/admin/audit-logs")]
[Authorize]
public class AdminAuditLogsController : ControllerBase
{
    private readonly IFourDigitCoreAdminService _adminService;

    public AdminAuditLogsController(IFourDigitCoreAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<AuditLog>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAuditLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default)
    {
        var logs = await _adminService.GetAuditLogsAsync(page, pageSize, cancellationToken);
        return Ok(logs);
    }
}
