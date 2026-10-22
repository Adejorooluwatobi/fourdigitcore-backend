using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Application.DTOs.Admin;
using FourDigitCore.Domain.Entities;

namespace FourDigitCore.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/solutions")]
[Route("api/v1/admin/solutions")]
[Authorize]
[Produces("application/json")]
public class AdminSolutionsController : ControllerBase
{
    private readonly IAppDbContext _context;

    public AdminSolutionsController(IAppDbContext context)
    {
        _context = context;
    }

    // 1. List All Solution Disciplines
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSolutions(CancellationToken cancellationToken)
    {
        var solutions = await _context.SolutionDisciplines
            .AsNoTracking()
            .Include(s => s.Specs)
            .Include(s => s.Applications)
            .OrderBy(s => s.SortOrder)
            .ToListAsync(cancellationToken);

        return Ok(solutions);
    }

    // 2. Get Single Solution Discipline
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSolutionById(Guid id, CancellationToken cancellationToken)
    {
        var solution = await _context.SolutionDisciplines
            .Include(s => s.Specs)
            .Include(s => s.Applications)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (solution == null)
            return NotFound(new { message = "Solution discipline not found." });

        return Ok(solution);
    }

    // 3. Create Solution Discipline
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSolution([FromBody] UpsertSolutionDisciplineDto dto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new { message = "Title is required." });

        var discipline = new SolutionDiscipline
        {
            Id = Guid.NewGuid(),
            DisciplineNumber = dto.DisciplineNumber,
            CategoryName = dto.CategoryName,
            Title = dto.Title,
            Description = dto.Description,
            CtaText = dto.CtaText,
            CtaUrl = dto.CtaUrl,
            SortOrder = dto.SortOrder
        };

        if (dto.Specs != null && dto.Specs.Count > 0)
        {
            int order = 0;
            foreach (var spec in dto.Specs)
            {
                discipline.Specs.Add(new SolutionDisciplineSpec
                {
                    Id = Guid.NewGuid(),
                    SpecText = spec,
                    SortOrder = ++order
                });
            }
        }

        if (dto.Applications != null && dto.Applications.Count > 0)
        {
            int order = 0;
            foreach (var app in dto.Applications)
            {
                discipline.Applications.Add(new SolutionDisciplineApplication
                {
                    Id = Guid.NewGuid(),
                    ApplicationName = app,
                    SortOrder = ++order
                });
            }
        }

        await _context.SolutionDisciplines.AddAsync(discipline, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetSolutionById), new { id = discipline.Id }, discipline);
    }

    // 4. Update Solution Discipline
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSolution(Guid id, [FromBody] UpsertSolutionDisciplineDto dto, CancellationToken cancellationToken)
    {
        var discipline = await _context.SolutionDisciplines
            .Include(s => s.Specs)
            .Include(s => s.Applications)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (discipline == null)
            return NotFound(new { message = "Solution discipline not found." });

        discipline.DisciplineNumber = dto.DisciplineNumber;
        discipline.CategoryName = dto.CategoryName;
        discipline.Title = dto.Title;
        discipline.Description = dto.Description;
        discipline.CtaText = dto.CtaText;
        discipline.CtaUrl = dto.CtaUrl;
        discipline.SortOrder = dto.SortOrder;
        discipline.UpdatedAt = DateTime.UtcNow;

        // Replace specs
        _context.SolutionDisciplineSpecs.RemoveRange(discipline.Specs);
        if (dto.Specs != null)
        {
            int order = 0;
            foreach (var spec in dto.Specs)
            {
                discipline.Specs.Add(new SolutionDisciplineSpec
                {
                    Id = Guid.NewGuid(),
                    SpecText = spec,
                    SortOrder = ++order
                });
            }
        }

        // Replace applications
        _context.SolutionDisciplineApplications.RemoveRange(discipline.Applications);
        if (dto.Applications != null)
        {
            int order = 0;
            foreach (var app in dto.Applications)
            {
                discipline.Applications.Add(new SolutionDisciplineApplication
                {
                    Id = Guid.NewGuid(),
                    ApplicationName = app,
                    SortOrder = ++order
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Ok(discipline);
    }

    // 5. Delete Solution Discipline
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSolution(Guid id, CancellationToken cancellationToken)
    {
        var discipline = await _context.SolutionDisciplines
            .Include(s => s.Specs)
            .Include(s => s.Applications)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (discipline == null)
            return NotFound(new { message = "Solution discipline not found." });

        _context.SolutionDisciplineSpecs.RemoveRange(discipline.Specs);
        _context.SolutionDisciplineApplications.RemoveRange(discipline.Applications);
        _context.SolutionDisciplines.Remove(discipline);

        await _context.SaveChangesAsync(cancellationToken);
        return Ok(new { success = true, message = "Solution discipline deleted." });
    }
}
