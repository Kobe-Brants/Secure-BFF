using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers(CancellationToken cancellationToken)
    {
        var users = new List<User>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new() { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" },
        };
        
        return Ok(users);
    }
}