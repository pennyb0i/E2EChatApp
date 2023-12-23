using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace E2EChatApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase{

    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    #region GET
    
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _userService.GetUserById(id);
        return Ok(user);
    }
    
    #endregion
    
    [HttpGet("HelloWorld")]
    public IActionResult Get()
    {
        return Ok("Hello world!");
    }
}
