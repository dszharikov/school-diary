using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Term.Data;
using Term.DTOs;

namespace Term.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TermController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TermController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TermOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTerms()
    {
        var terms = await _context.Terms.ToListAsync();

        if (terms == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<TermOutputDto>>(terms));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TermOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTerm(int id)
    {
        var term = await _context.Terms.FindAsync(id);

        if (term == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<TermOutputDto>(term));
    }

    [HttpGet("school/{schoolId}")]
    [ProducesResponseType(typeof(IEnumerable<TermOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTermsBySchool(int schoolId)
    {
        var terms = await _context.Terms.Where(t => t.SchoolId == schoolId).ToListAsync();

        if (terms == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<TermOutputDto>>(terms));
    }

    [HttpPost]
    [ProducesResponseType(typeof(TermOutputDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTerm([FromBody] CreateTermDto termDto)
    {
        if (String.IsNullOrWhiteSpace(termDto.Name) 
            || String.IsNullOrWhiteSpace(termDto.StartDate) 
            || String.IsNullOrWhiteSpace(termDto.EndDate))
        {
            return BadRequest("Name, StartDate and EndDate are required");
        }

        Models.Term term;
        try
        {
            term = _mapper.Map<Models.Term>(termDto);
        }
        catch(Exception)
        {
            return BadRequest("Invalid StartDate or EndDate format. Use yyyy-MM-dd");
        }

        if (term.StartDate >= term.EndDate)
        {
            return BadRequest("StartDate must be before EndDate");
        }

        _context.Terms.Add(term);

        await _context.SaveChangesAsync();

        return CreatedAtAction
                (
                    nameof(GetTerm),
                    new { term.Id },
                    _mapper.Map<TermOutputDto>(term)
                );
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTerm(int id, [FromBody] UpdateTermDto termDto)
    {
        if (String.IsNullOrWhiteSpace(termDto.Name) || String.IsNullOrWhiteSpace(termDto.StartDate) || String.IsNullOrWhiteSpace(termDto.EndDate))
        {
            return BadRequest("Name, StartDate and EndDate are required");
        }

        var term = await _context.Terms.FindAsync(id);

        if (term is null)
        {
            return NotFound();
        }

        term.Name = termDto.Name;

        if (DateOnly.TryParse(termDto.StartDate, out var startDate)
            && DateOnly.TryParse(termDto.EndDate, out var endDate)
            && startDate < endDate)
        {
            term.StartDate = startDate;
            term.EndDate = endDate;
        }
        else
        {
            return BadRequest("Invalid StartDate or EndDate");
        }

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTerm(int id)
    {
        var term = await _context.Terms.FindAsync(id);

        if (term == null)
        {
            return NotFound();
        }

        _context.Terms.Remove(term);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}