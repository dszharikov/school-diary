using Microsoft.AspNetCore.Mvc;
using User.Data.Repositories.ParentRepositories;

namespace User.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParentController : ControllerBase
{
    private readonly IParentRepository _parentRepository;

    public ParentController(IParentRepository parentRepository)
    {
        _parentRepository = parentRepository;   
    }
    
    [HttpGet("{parentId}/student")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentId(int parentId)
    {
        var studentId = await _parentRepository.GetStudentId(parentId);

        if (studentId == null)
        {
            return NotFound();
        }
        return Ok(studentId);
    }
}