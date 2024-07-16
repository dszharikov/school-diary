using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Grade.Models;
using Grade.Data;
using AutoMapper;
using Grade.DTOs.Input.TermAssessments;
using Grade.DTOs.Output;

namespace Grade.Controllers;

/// <summary>
/// API controller for managing term assessments.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class TermAssessmentController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TermAssessmentController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all term assessments.
    /// </summary>
    /// <returns>A list of term assessments.</returns>
    /// <remarks>
    /// Check what it returns
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TermAssessmentOutputDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetTermAssessments()
    {
        var termAssessmentOutputDTO = _mapper.Map<IEnumerable<TermAssessmentOutputDTO>>(
            await _context.TermAssessments.ToListAsync());

        return Ok(termAssessmentOutputDTO);
    }

    /// <summary>
    /// Retrieves a specific term assessment by its ID.
    /// </summary>
    /// <param name="id">The ID of the term assessment.</param>
    /// <returns>The term assessment with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TermAssessmentOutputDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetTermAssessment(int id)
    {
        var termAssessment = await _context.TermAssessments.FindAsync(id);

        if (termAssessment == null)
        {
            return NotFound();
        }

        var termAssessmentOutputDTO = _mapper.Map<TermAssessmentOutputDTO>(termAssessment);

        return Ok(termAssessmentOutputDTO);
    }

    /// <summary>
    /// Creates a new term assessment.
    /// </summary>
    /// <param name="termAssessmentDto">The term assessment to create.</param>
    /// <returns>The created term assessment.</returns>
    /// <remarks>
    /// Check what it returns
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(TermAssessmentOutputDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> PostTermAssessment(
        [FromBody] CreateTermAssessmentDTO termAssessmentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var termAssessment = _mapper.Map<TermAssessment>(termAssessmentDto);

        _context.TermAssessments.Add(termAssessment);
        await _context.SaveChangesAsync();

        var termAssessmentOutputDTO = _mapper.Map<TermAssessmentOutputDTO>(termAssessment);

        return CreatedAtAction(nameof(GetTermAssessment), new { id = termAssessmentOutputDTO.Id }, termAssessmentOutputDTO);
    }

    /// <summary>
    /// Updates an existing term assessment.
    /// </summary>
    /// <param name="id">The ID of the term assessment to update.</param>
    /// <param name="termAssessmentDto">The updated term assessment.</param>
    /// <returns>No content if the update is successful.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutTermAssessment(
        int id, [FromBody] UpdateTermAssessmentDTO termAssessmentDto)
    {
        if (id != termAssessmentDto.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var termAssessment = await _context.TermAssessments.FindAsync(id);

        if (termAssessment == null)
        {
            return NotFound();
        }

        _mapper.Map(termAssessmentDto, termAssessment);

        _context.TermAssessments.Update(termAssessment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes a term assessment.
    /// </summary>
    /// <param name="id">The ID of the term assessment to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTermAssessment(int id)
    {
        var termAssessment = await _context.TermAssessments.FindAsync(id);
        if (termAssessment == null)
        {
            return NotFound();
        }

        _context.TermAssessments.Remove(termAssessment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
