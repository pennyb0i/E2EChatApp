using System.Net;
using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace E2EChatApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class FriendshipController : ControllerBase{
    private readonly IFriendshipService _friendshipService;
    public FriendshipController(IFriendshipService friendshipService)
    {
        _friendshipService = friendshipService;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllFriendships()
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim is null) {
            throw new RestException(HttpStatusCode.NotFound, "Current user Id not found");
        }
        var friendships = await _friendshipService.GetAllFriendshipsByUserId(int.Parse(userIdClaim.Value));
        return Ok(friendships);
    }
    
    [Authorize]
    [HttpPost("{userId:int}")]
    public async Task<IActionResult> CreateFriendship(int userId)
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim is null) {
            throw new RestException(HttpStatusCode.NotFound, "Current user Id not found");
        }
        var wasCreated = await _friendshipService.CreateFriendship(int.Parse(userIdClaim.Value),userId);
        return wasCreated ? NoContent() : throw new RestException(HttpStatusCode.BadRequest, "Failed to create friendship");
    }
    
    [Authorize]
    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> CancelFriendship(int userId)
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim is null) {
            throw new RestException(HttpStatusCode.NotFound, "Current user Id not found");
        }
        var wasCancelled = await _friendshipService.CancelFriendship(int.Parse(userIdClaim.Value),userId);
        return wasCancelled ? NoContent() : throw new RestException(HttpStatusCode.BadRequest, "Failed to cancel friendship");
    }
}
