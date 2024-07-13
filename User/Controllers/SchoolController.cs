using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Data;
using User.DTOs.Input;
using User.DTOs.Output;
using User.Models;

namespace User.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SchoolController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SchoolController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SchoolOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSchools()
    {
        var schools = await _context.Schools.ToListAsync();

        if (schools == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<SchoolOutputDto>>(schools));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SchoolOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSchool(int id)
    {
        var school = await _context.Schools.FirstOrDefaultAsync(school => school.Id == id);

        if (school == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<SchoolOutputDto>(school));
    }

    [HttpPost]
    [ProducesResponseType(typeof(SchoolOutputDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSchool([FromBody] SchoolInputDto schoolDto)
    {
        if (String.IsNullOrWhiteSpace(schoolDto.Name) || String.IsNullOrWhiteSpace(schoolDto.Address))
        {
            return BadRequest("Name and Address are required");
        }

        var school = new School
        {
            Name = schoolDto.Name,
            Address = schoolDto.Address
        };

        _context.Schools.Add(school);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSchool), new { school.Id }, _mapper.Map<SchoolOutputDto>(school));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSchool(int id, [FromBody] SchoolInputDto schoolDto)
    {
        if (String.IsNullOrWhiteSpace(schoolDto.Name) || String.IsNullOrWhiteSpace(schoolDto.Address))
        {
            return BadRequest("Name and Address are required");
        }

        var school = await _context.Schools.FindAsync(id);

        if (school is null)
        {
            return NotFound();
        }

        school.Name = schoolDto.Name;
        school.Address = schoolDto.Address;

        _context.Schools.Update(school);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSchool(int id)
    {
        var school = await _context.Schools.FindAsync(id);

        if (school is null)
        {
            return NotFound();
        }

        _context.Schools.Remove(school);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}