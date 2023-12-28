using System.Net;
using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace E2EChatApp.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    #region GET
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] bool? friendsOnly = null)
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim is null) {
            throw new RestException(HttpStatusCode.NotFound, "Current user Id not found");
        }
        var user = await _userService.GetAllUsers(friendsOnly,int.Parse(userIdClaim.Value));
        return Ok(user);
    }
    
    [Authorize]
    [HttpGet("currentUser")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim is null) {
            throw new RestException(HttpStatusCode.NotFound, "Current user Id not found");
        }
        var user = await _userService.GetUserById(int.Parse(userIdClaim.Value));
        return Ok(user);
    }
    
    #endregion
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetUsers();

        return Ok(users);
    }
}
