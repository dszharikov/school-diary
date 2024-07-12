using Microsoft.AspNetCore.Mvc;
using User.Data.Repositories.SchoolRepositories;
using User.DTOs.Input;
using User.Models;

namespace User.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolController : ControllerBase
{
    private readonly ISchoolRepository _schoolRepository;

    public SchoolController(ISchoolRepository schoolRepository)
    {
        _schoolRepository = schoolRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<School>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSchools()
    {
        var schools = await _schoolRepository.GetSchools();

        if (schools == null)
        {
            return NotFound();
        }
        return Ok(schools);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(School), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSchool(int id)
    {
        var school = await _schoolRepository.GetSchool(id);

        if (school == null)
        {
            return NotFound();
        }
        return Ok(school);
    }

    [HttpPost]
    [ProducesResponseType(typeof(School), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSchool([FromBody] SchoolInputDto schoolDto)
    {
        if (String.IsNullOrWhiteSpace(schoolDto.Name) || String.IsNullOrWhiteSpace(schoolDto.Address))
        {
            return BadRequest("Name and Address are required");
        }

        var id = await _schoolRepository.CreateSchool(schoolDto);

        return CreatedAtAction(nameof(GetSchool), new { id },
            new School { Address = schoolDto.Address, Id = id, Name = schoolDto.Name });
    }
}