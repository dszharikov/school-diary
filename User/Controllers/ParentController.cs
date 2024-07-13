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
public class ParentController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ParentController(AppDbContext context, IMapper mapper)
    {
        _context = context;   
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ParentOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetParents()
    {
        var parents = await _context.Parents.ToListAsync();

        if (parents == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<IEnumerable<ParentOutputDto>>(parents));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ParentOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetParent(int id)
    {
        var parent = await _context.Parents.FirstOrDefaultAsync(parent => parent.Id == id);

        if (parent == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ParentOutputDto>(parent));
    }


    [HttpGet("{parentId}/student")]
    [ProducesResponseType(typeof(UserOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudent(int parentId)
    {
        var parent = await _context.Parents.FirstOrDefaultAsync(parent => parent.Id == parentId);

        if (parent == null)
        {
            return NotFound();
        }

        var student = await _context.Users.FirstOrDefaultAsync(user => user.Id == parent.StudentId);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<UserOutputDto>(student));
    }

    [HttpGet("school/{schoolId}/")]
    [ProducesResponseType(typeof(IEnumerable<ParentOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetParentsBySchool(int schoolId)
    {
        var parents = await _context.Parents
            .Where(parent => parent.SchoolId == schoolId).ToListAsync();

        if (parents == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<ParentOutputDto>>(parents));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ParentOutputDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddParent([FromBody] ParentInputDto parentDto)
    {
        if (String.IsNullOrWhiteSpace(parentDto.Name) || String.IsNullOrWhiteSpace(parentDto.Email))
        {
            return BadRequest("Name and Email are required");
        }

        var school = await _context.Schools.FindAsync(parentDto.SchoolId);

        if (school == null)
        {
            return NotFound("School not found");
        }

        var student = await _context.Users.FindAsync(parentDto.StudentId);

        if (student == null)
        {
            return NotFound("Student not found");
        }

        var parent = _mapper.Map<Parent>(parentDto);

        await _context.Parents.AddAsync(parent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetParent), new { parent.Id }, _mapper.Map<ParentOutputDto>(parent));
    }

    

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ParentOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateParent(int id, [FromBody] ParentInputDto parentDto)
    {
        if (String.IsNullOrWhiteSpace(parentDto.Name) || String.IsNullOrWhiteSpace(parentDto.Email))
        {
            return BadRequest("Name and Email are required");
        }

        var parent = await _context.Parents.FirstOrDefaultAsync(parent => parent.Id == id);

        if (parent == null)
        {
            return NotFound();
        }

        if (parent.SchoolId != parentDto.SchoolId)
        {
            var school = await _context.Schools.FindAsync(parentDto.SchoolId);

            if (school == null)
            {
                return NotFound("School not found");
            }
        }

        if (parent.StudentId != parentDto.StudentId)
        {
            var student = await _context.Users.FirstOrDefaultAsync(user => user.Id == parentDto.StudentId);

            if (student == null)
            {
                return NotFound("Student not found");
            }
        }

        parent.Name = parentDto.Name;
        parent.Email = parentDto.Email;
        parent.SchoolId = parentDto.SchoolId;
        parent.StudentId = parentDto.StudentId;

        _context.Parents.Update(parent);

        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<ParentOutputDto>(parent));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteParent(int id)
    {
        var parent = await _context.Parents.FirstOrDefaultAsync(parent => parent.Id == id);

        if (parent == null)
        {
            return NotFound();
        }

        _context.Parents.Remove(parent);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}