using AutoMapper;
using Homework.Data;
using Homework.DTOs;
using Homework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Homework.Controllers;

/// <summary>
/// Controller for Homeworks
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class HomeworkController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITermService _termService;

    private DateOnly GetActualDate()
    {
        return TimeOnly.FromDateTime(DateTime.Now) > new TimeOnly(14, 0, 0)
            ? DateOnly.FromDateTime(DateTime.Now.AddDays(1))
            : DateOnly.FromDateTime(DateTime.Now);
    }

    public HomeworkController(AppDbContext context, IMapper mapper, ITermService termService)
    {
        _context = context;
        _mapper = mapper;
        _termService = termService;
    }

    /// <summary>
    /// Get all homeworks
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Models.Homework>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var homeworks = await _context.Homeworks.ToListAsync();
        return Ok(homeworks);
    }

    /// <summary>
    /// Get a homework by id
    /// </summary>
    /// <param name="id">homework id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Models.Homework), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        int id)
    {
        var homework = await _context.Homeworks.FirstOrDefaultAsync(h => h.Id == id);

        if (homework is null)
        {
            return NotFound();
        }

        return Ok(homework);
    }

    /// <summary>
    /// Get all homeworks for a classSubject
    /// </summary>
    /// <param name="classSubjectId">classSubject id</param>
    /// <returns></returns>
    // get all homeworks for a classSubject
    [HttpGet("classSubject/{classSubjectId}")]
    [ProducesResponseType(typeof(IEnumerable<Models.Homework>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByClassSubject(
        int classSubjectId)
    {
        var homeworks = await _context.Homeworks
            .Where(h => h.ClassSubjectId == classSubjectId)
            .ToListAsync();

        return Ok(homeworks);
    }

    /// <summary>
    /// Get all homeworks for a classSubject and term
    /// </summary>
    /// <param name="classSubjectId">classSubject id</param>
    /// <param name="termId">term id</param>
    /// <returns></returns>
    [HttpGet("classSubject/{classSubjectId}/term/{termId}")]
    [ProducesResponseType(typeof(IEnumerable<Models.Homework>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByClassSubjectsAndTerm(
        int classSubjectId, int termId)
    {
        var term = await _termService.GetTermAsync(termId);

        if (term is null)
        {
            return NotFound();
        }

        var homeworks = await _context.Homeworks
            .Where(h => h.ClassSubjectId == classSubjectId
                && h.DueDate >= term.StartDate
                && h.DueDate <= term.EndDate)
            .ToListAsync();

        return Ok(homeworks);
    }

    /// <summary>
    /// Get all homeworks for a classSubject and date range
    /// </summary>
    /// <param name="classSubjectId">classSubject id</param>
    /// <param name="startDay">start date</param>
    /// <param name="endDay">end date</param>
    /// <returns></returns>
    [HttpGet("classSubject/{classSubjectId}/startDay/{startDay}/endDay/{endDay}")]
    [ProducesResponseType(typeof(IEnumerable<Models.Homework>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByClassSubjectsAndDate(
        int classSubjectId, DateOnly startDay, DateOnly endDay)
    {
        var homeworks = await _context.Homeworks
            .Where(h => h.ClassSubjectId == classSubjectId
                && h.DueDate >= startDay
                && h.DueDate <= endDay)
            .ToListAsync();

        return Ok(homeworks);
    }

    /// <summary>
    /// Get all homeworks for a classSubject that are in date (due date is today or later)
    /// </summary>
    /// <param name="classSubjectId">classSubject id</param>
    /// <returns></returns>
    [HttpGet("classSubject/{classSubjectId}/indate")]
    [ProducesResponseType(typeof(IEnumerable<Models.Homework>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInDateByClassSubjects(
        int classSubjectId)
    {
        var actualDate = GetActualDate();

        var homeworks = await _context.Homeworks
            .Where(h => h.ClassSubjectId == classSubjectId && h.DueDate >= actualDate)
            .ToListAsync();

        return Ok(homeworks);
    }

    /// <summary>
    /// Get all homeworks for a classSubject that are in date (due date is today or later) and term
    /// </summary>
    /// <param name="classSubjectIds">classSubject ids (list or array)</param>
    /// <param name="termId">term id</param>
    /// <remarks>
    /// Sample classSubjectIds: [1, 2, 3]
    /// </remarks>
    /// <returns></returns>
    [HttpPost("classSubjects/term/{termId}/indate")]
    [ProducesResponseType(typeof(IEnumerable<Models.Homework>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInDateByClassSubjectsAndTerm(
        [FromBody] int[] classSubjectIds, int termId)
    {
        var term = await _termService.GetTermAsync(termId);

        if (term is null)
        {
            return NotFound();
        }

        var actualDate = GetActualDate();

        var homeworks = await _context.Homeworks
            .Where(h => classSubjectIds.Contains(h.ClassSubjectId)
                && h.DueDate >= actualDate
                && h.DueDate <= term.EndDate)
            .ToListAsync();

        return Ok(homeworks);
    }

    /// <summary>
    /// Get all homeworks for a classSubject that are in date (due date is today or later)
    /// </summary>
    /// <param name="classSubjectIds">classSubject ids (list or array)</param>
    /// <remarks>
    /// Sample classSubjectIds: [1, 2, 3]
    /// </remarks>
    /// <returns></returns>
    [HttpPost("classSubjects/indate")]
    [ProducesResponseType(typeof(IEnumerable<Models.Homework>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInDateByClassSubjects(
        [FromBody] int[] classSubjectIds)
    {
        var actualDate = GetActualDate();

        var homeworks = await _context.Homeworks
            .Where(h => classSubjectIds.Contains(h.ClassSubjectId) 
                && h.DueDate >= actualDate)
            .ToListAsync();

        return Ok(homeworks);
    }

    /// <summary>
    /// Create a new homework
    /// </summary>
    /// <param name="homeworkDto">homework data</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Models.Homework), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(
        CreateHomeworkDTO homeworkDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var homework = _mapper.Map<Models.Homework>(homeworkDto);

        _context.Homeworks.Add(homework);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = homework }, homework);
    }

    /// <summary>
    /// Update a homework
    /// </summary>
    /// <param name="id">homework id</param>
    /// <param name="homeworkDto">homework data</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(
        int id, UpdateHomeworkDTO homeworkDto)
    {
        if (id != homeworkDto.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var homework = await _context.Homeworks.FirstOrDefaultAsync(h => h.Id == id);

        if (homework == null)
        {
            return NotFound();
        }

        _mapper.Map(homeworkDto, homework);

        _context.Homeworks.Update(homework);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete a homework
    /// </summary>
    /// <param name="id">homework id</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id)
    {
        var homework = await _context.Homeworks.FirstOrDefaultAsync(h => h.Id == id);

        if (homework == null)
        {
            return NotFound();
        }

        _context.Homeworks.Remove(homework);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}