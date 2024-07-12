using Microsoft.AspNetCore.Mvc;
using User.Data.Repositories.SchoolRepositories;
using User.Data.Repositories.UserRepositories;
using User.DTOs.Input;

namespace User.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ISchoolRepository _schoolRepository;

    public UserController(IUserRepository userRepository, ISchoolRepository schoolRepository)
    {
        _userRepository = userRepository;
        _schoolRepository = schoolRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Models.User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetUsers();

        if (users == null)
        {
            return NotFound();
        }
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Models.User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userRepository.GetUserById(id);

        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet("school/{schoolId}")]
    [ProducesResponseType(typeof(IEnumerable<Models.User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsersBySchool(int schoolId)
    {
        var school = await _schoolRepository.GetSchool(schoolId);

        if (school == null)
        {
            return NotFound("School not found");
        }

        var users = await _userRepository.GetUsersBySchool(schoolId);

        if (users == null)
        {
            return NotFound();
        }
        return Ok(users);
    }

    [HttpGet("school/{schoolId}/teachers")]
    [ProducesResponseType(typeof(IEnumerable<Models.User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTeachersBySchool(int schoolId)
    {
        var school = await _schoolRepository.GetSchool(schoolId);

        if (school == null)
        {
            return NotFound("School not found");
        }

        var teachers = await _userRepository.GetTeachersBySchool(schoolId);

        if (teachers == null)
        {
            return NotFound();
        }
        return Ok(teachers);
    }

    [HttpGet("school/{schoolId}/students")]
    [ProducesResponseType(typeof(IEnumerable<Models.User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentsBySchool(int schoolId)
    {
        var school = await _schoolRepository.GetSchool(schoolId);

        if (school == null)
        {
            return NotFound("School not found");
        }

        var students = await _userRepository.GetStudentsBySchool(schoolId);

        if (students == null)
        {
            return NotFound();
        }
        return Ok(students);
    }

    [HttpGet("school/{schoolId}/parents")]
    [ProducesResponseType(typeof(IEnumerable<Models.User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetParentsBySchool(int schoolId)
    {
        var school = await _schoolRepository.GetSchool(schoolId);

        if (school == null)
        {
            return NotFound("School not found");
        }

        var parents = await _userRepository.GetParentsBySchool(schoolId);

        if (parents == null)
        {
            return NotFound();
        }
        return Ok(parents);
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

        var school = await _schoolRepository.GetSchool(userDto.SchoolID);

        if (school == null)
        {
            return NotFound("School not found");
        }

        var id = await _userRepository.CreateUser(userDto);

        if (id is null)
        {
            return UnprocessableEntity();
        }

        return CreatedAtAction(nameof(GetUser), new { id }, userDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserInputDto user)
    {
        if (String.IsNullOrWhiteSpace(user.Name) || String.IsNullOrWhiteSpace(user.Email) || String.IsNullOrWhiteSpace(user.Role))
        {
            return BadRequest("Name, Email and Role are required");
        }

        var success = await _userRepository.UpdateUser(id, user);

        if (!success)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await _userRepository.DeleteUser(id);

        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}
