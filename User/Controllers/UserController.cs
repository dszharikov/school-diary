using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Data;
using User.DTOs.Input;
using User.DTOs.Output;

namespace User.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.ToListAsync();

        if (users == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<UserOutputDto>>(users));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserOutputDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<UserOutputDto>(user));
    }

    [HttpGet("school/{schoolId}")]
    [ProducesResponseType(typeof(IEnumerable<UserOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsersBySchool(int schoolId)
    {
        var users = await _context.Users.Where(user => user.SchoolId == schoolId).ToListAsync();

        if (users == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<UserOutputDto>>(users));
    }

    [HttpGet("school/{schoolId}/teachers")]
    [ProducesResponseType(typeof(IEnumerable<UserOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeachersBySchool(int schoolId)
    {
        var teachers = await _context.Users
            .Where(user => user.SchoolId == schoolId && user.Role == Constants.TeacherRoleName).ToListAsync();

        if (teachers == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<UserOutputDto>>(teachers));
    }

    [HttpGet("school/{schoolId}/students")]
    [ProducesResponseType(typeof(IEnumerable<UserOutputDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentsBySchool(int schoolId)
    {
        var students = await _context.Users
            .Where(user => user.SchoolId == schoolId && user.Role == Constants.StudentRoleName).ToListAsync();

        if (students == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<IEnumerable<UserOutputDto>>(students));
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddUser([FromBody] UserInputDto userDto)
    {
        if (String.IsNullOrWhiteSpace(userDto.Name) || String.IsNullOrWhiteSpace(userDto.Email)
            || String.IsNullOrWhiteSpace(userDto.Role))
        {
            return BadRequest("Name, Email and Role are required");
        }

        if (userDto.Role == Constants.ParentRoleName)
        {
            return BadRequest("Use parent endpoint: /api/parent");
        }

        var school = await _context.Schools.FindAsync(userDto.SchoolId);

        if (school is null)
        {
            return NotFound("School not found");
        }

        var user = _mapper.Map<Models.User>(userDto);

        _context.Users.Add(user);

        var changedEntities = await _context.SaveChangesAsync();

        if (changedEntities == 0)
        {
            return UnprocessableEntity();
        }

        return CreatedAtAction(nameof(GetUser), new { user.Id }, _mapper.Map<UserOutputDto>(user));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserInputDto userDto)
    {
        if (String.IsNullOrWhiteSpace(userDto.Name)
            || String.IsNullOrWhiteSpace(userDto.Email)
            || String.IsNullOrWhiteSpace(userDto.Role))
        {
            return BadRequest("Name, Email and Role are required");
        }

        var user = await _context.Users.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        if (user.SchoolId != userDto.SchoolId)
        {
            var school = await _context.Schools.FindAsync(userDto.SchoolId);

            if (school is null)
            {
                return NotFound("School not found");
            }
        }

        user.Email = userDto.Email;
        user.Name = userDto.Name;
        user.Role = userDto.Role;
        user.SchoolId = userDto.SchoolId;

        _context.Users.Update(user);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
