using Microsoft.AspNetCore.Mvc;
namespace E2EChatApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase{

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet(Name = "HelloWorld")]
    public IActionResult Get()
    {
        return Ok("Hello world!");
    }
}
