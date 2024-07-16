using AutoMapper;
using Grade.Data;
using Grade.DTOs.Input.QuarterlyGrades;
using Grade.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grade.Controllers;


/// <summary>
/// API controller for managing quarterly grades.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class QuarterlyGradeController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public QuarterlyGradeController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all quarterly grades.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<QuarterlyGrade>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuarterlyGrades()
    {
        var quarterlyGrades = await _context.QuarterlyGrades.ToListAsync();
        return Ok(quarterlyGrades);
    }

    /// <summary>
    /// Retrieves a specific quarterly grade by its ID.
    /// </summary>
    /// <param name="id">The ID of the quarterly grade.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(QuarterlyGrade), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuarterlyGrade(int id)
    {
        var quarterlyGrade = await _context.QuarterlyGrades.FindAsync(id);

        if (quarterlyGrade == null)
        {
            return NotFound();
        }

        return Ok(quarterlyGrade);
    }

    /// <summary>
    /// Retrieves all quarterly grades for a specific student.
    /// </summary>
    /// <param name="studentId">The ID of the student.</param>
    /// <param name="termId">The ID of the term.</param>
    /// <returns></returns>
    [HttpGet("student/{studentId}/term/{termId}")]
    [ProducesResponseType(typeof(IEnumerable<QuarterlyGrade>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuarterlyGradesByStudent(int studentId, int termId)
    {
        var quarterlyGrades = await _context.QuarterlyGrades
            .Where(qg => qg.StudentId == studentId && qg.TermId == termId)
            .ToListAsync();

        return Ok(quarterlyGrades);
    }

    /// <summary>
    /// Creates a new quarterly grade.
    /// </summary>
    /// <param name="quarterlyGradeDTO">The quarterly grade to create.</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(QuarterlyGrade), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateQuarterlyGrade(CreateQuarterlyGradeDTO quarterlyGradeDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var quarterlyGrade = _mapper.Map<Models.QuarterlyGrade>(quarterlyGradeDTO);

        _context.QuarterlyGrades.Add(quarterlyGrade);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetQuarterlyGrade), new { id = quarterlyGrade.Id }, quarterlyGrade);
    }

    /// <summary>
    /// Updates an existing quarterly grade.
    /// </summary>
    /// <param name="id">The ID of the quarterly grade to update.</param>
    /// <param name="quarterlyGradeDTO">The updated quarterly grade data.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateQuarterlyGrade(int id, UpdateQuarterlyGradeDTO quarterlyGradeDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != quarterlyGradeDTO.Id)
        {
            return BadRequest();
        }

        var quarterlyGrade = await _context.QuarterlyGrades.FindAsync(id);

        if (quarterlyGrade == null)
        {
            return NotFound();
        }

        _mapper.Map(quarterlyGradeDTO, quarterlyGrade);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}