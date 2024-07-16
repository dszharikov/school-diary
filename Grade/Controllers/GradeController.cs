using AutoMapper;
using Grade.Data;
using Grade.DTOs.Input.Grades;
using Grade.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grade.Controllers;

/// <summary>
/// API controller for managing grades.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class GradeController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITermService _termService;

    public GradeController(AppDbContext context, IMapper mapper, ITermService termService)
    {
        _context = context;
        _mapper = mapper;
        _termService = termService;
    }

    /// <summary>
    /// Retrieves all grades.
    /// </summary>
    /// <returns>A list of grades.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Models.Grade>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGrades()
    {
        var grades = await _context.Grades.ToListAsync();
        return Ok(grades);
    }

    /// <summary>
    /// Retrieves a specific grade by its ID.
    /// </summary>
    /// <param name="id">The ID of the grade.</param>
    /// <returns>The grade with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Models.Grade), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGrade(int id)
    {
        var grade = await _context.Grades.FindAsync(id);
        if (grade == null)
        {
            return NotFound();
        }

        return Ok(grade);
    }

    /// <summary>
    /// Retrieves all grades for a specific student.
    /// </summary>
    /// <param name="studentId">The ID of the student.</param>
    /// <param name="termId">The ID of the term.</param>
    /// <returns>A list of grades for the specified student.</returns>
    /// <remarks>
    /// Sends request to Term service to get term details.
    /// </remarks>
    [HttpGet("student/{studentId}/term/{termId}")]
    [ProducesResponseType(typeof(IEnumerable<Models.Grade>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGradesByStudentId(int studentId, int termId)
    {
        var term = await _termService.GetTermAsync(termId);

        if (term == null)
        {
            return NotFound();
        }

        var grades = await _context.Grades
            .Where(g => g.StudentId == studentId
                && g.Date >= term.StartDate
                && g.Date <= term.EndDate)
            .ToListAsync();
        return Ok(grades);
    }

    /// <summary>
    /// Retrieves all grades for a specific student and subject.
    /// </summary>
    /// <param name="studentId">The ID of the student.</param>
    /// <param name="classSubjectId">The ID of the classSubject.</param>
    /// <returns>A list of grades for the specified student and subject.</returns>
    [HttpGet("student/{studentId}/subject/{classSubjectId}")]
    [ProducesResponseType(typeof(IEnumerable<Models.Grade>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGradesByStudentIdAndSubjectId(int studentId, int classSubjectId)
    {
        var grades = await _context.Grades
            .Where(g => g.StudentId == studentId && g.ClassSubjectId == classSubjectId)
            .ToListAsync();
        return Ok(grades);
    }

    /// <summary>
    /// Retrieves all grades for a specific class subject.
    /// </summary>
    /// <param name="classSubjectId">The ID of the class subject.</param>
    /// <param name="termId">The ID of the term.</param>
    /// <returns>A list of grades for the specified class subject.</returns>    
    /// <remarks>
    /// Sends request to Term service to get term details.
    /// </remarks>
    [HttpGet("classsubject/{classSubjectId}/term/{termId}")]
    [ProducesResponseType(typeof(IEnumerable<Models.Grade>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGradesByClassSubjectId(int classSubjectId, int termId)
    {
        var term = await _termService.GetTermAsync(termId);

        if (term == null)
        {
            return NotFound();
        }

        var grades = await _context.Grades
            .Where(g => g.ClassSubjectId == classSubjectId
                && g.Date >= term.StartDate
                && g.Date <= term.EndDate)
            .ToListAsync();
        return Ok(grades);
    }

    /// <summary>
    /// Creates a new grade.
    /// </summary>
    /// <param name="createGradeDto">The grade to create.</param>
    /// <returns>The created grade.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Models.Grade), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGrade([FromBody] CreateGradeDto createGradeDto)
    {
        if (ModelState.IsValid)
        {
            var grade = _mapper.Map<Models.Grade>(createGradeDto);

            _context.Grades.Add(grade);

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGrade), new { id = grade.Id }, grade);
        }
        return BadRequest(ModelState);
    }

    /// <summary>
    /// Updates an existing grade.
    /// </summary>
    /// <param name="id">The ID of the grade to update.</param>
    /// <param name="updateGradeDto">The updated grade.</param>
    /// <returns>No content if the update is successful.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGrade(int id, [FromBody] UpdateGradeDto updateGradeDto)
    {
        if (id != updateGradeDto.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var grade = await _context.Grades.FindAsync(id);

            if (grade == null)
            {
                return NotFound();
            }

            _mapper.Map(updateGradeDto, grade);

            _context.Grades.Update(grade);
            await _context.SaveChangesAsync();

            return Ok();
        }
        return BadRequest(ModelState);
    }

    /// <summary>
    /// Deletes a grade.
    /// </summary>
    /// <param name="id">The ID of the grade to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        var grade = await _context.Grades.FindAsync(id);
        if (grade == null)
        {
            return NotFound();
        }

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}