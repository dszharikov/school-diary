using Microsoft.AspNetCore.Mvc;
using Grade.Models;
using Grade.Data;
using Grade.DTOs.Input.AssessmentTypes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Grade.Controllers;

/// <summary>
/// API controller for managing assessment types.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AssessmentTypeController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssessmentTypeController"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="mapper">The mapper service</param>
    public AssessmentTypeController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all assessmentTypes.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAssessmentTypes()
    {
        var assessmentTypes = await _context.AssessmentTypes.ToListAsync();

        return Ok(assessmentTypes);
    }

    /// <summary>
    /// Retrieves a specific assessment type by its ID.
    /// </summary>
    /// <param name="id">The ID of the assessmentType.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssessmentType(int id)
    {
        var assessmentType = await _context.AssessmentTypes.FindAsync(id);

        if (assessmentType == null)
        {
            return NotFound();
        }

        return Ok(assessmentType);
    }

    /// <summary>
    /// Creates a new assessment type.
    /// </summary>
    /// <param name="createAssessmentTypeDTO">The DTO containing the assessment type data.</param>
    /// <returns>The created assessment type.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAssessmentType(CreateAssessmentTypeDTO createAssessmentTypeDTO)
    {
        // Validate the input DTO
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map the DTO to the model
        var assessmentType = _mapper.Map<AssessmentType>(createAssessmentTypeDTO);

        // Add the assessment type to the database
        _context.AssessmentTypes.Add(assessmentType);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAssessmentType), new { id = assessmentType.Id }, assessmentType);
    }

    /// <summary>
    /// Updates an existing assessment type.
    /// </summary>
    /// <param name="id">The ID of the assessment type to update.</param>
    /// <param name="updateAssessmentTypeDTO">The DTO containing the updated assessment type data.</param>
    /// <returns>The updated assessment type.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAssessmentType(int id, UpdateAssessmentTypeDTO updateAssessmentTypeDTO)
    {
        // Validate the input DTO
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != updateAssessmentTypeDTO.Id)
        {
            return BadRequest();
        }

        // Find the assessment type in the database
        var assessmentType = await _context.AssessmentTypes.FindAsync(id);

        // Check if the assessment type exists
        if (assessmentType == null)
        {
            return NotFound();
        }

        // Update the properties of the assessment type
        _mapper.Map(updateAssessmentTypeDTO, assessmentType);

        // Save the changes to the database
        _context.AssessmentTypes.Update(assessmentType);
        await _context.SaveChangesAsync();

        return Ok(assessmentType);
    }

    /// <summary>
    /// Deletes an assessment type.
    /// </summary>
    /// <param name="id">The ID of the assessment type to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAssessmentType(int id)
    {
        var assessmentType = await _context.AssessmentTypes.FindAsync(id);

        if (assessmentType == null)
        {
            return NotFound();
        }

        _context.AssessmentTypes.Remove(assessmentType);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}