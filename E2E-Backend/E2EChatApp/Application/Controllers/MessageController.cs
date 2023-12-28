using System.Net;
using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.BindingModels;
using E2EChatApp.Core.Domain.Exceptions;
using E2EChatApp.Core.Domain.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace E2EChatApp.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IMessageService _messageService;

    public MessageController(IHubContext<ChatHub> hubContext,IMessageService messageService)
    {
        _hubContext = hubContext;
        _messageService = messageService;
    }
    
    [Authorize]
    [HttpGet("{secondUserId}")]
    public async Task<IActionResult> Get(int secondUserId)
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim is null) {
            throw new RestException(HttpStatusCode.NotFound, "Current user Id not found");
        }
        var messages = await _messageService.GetMessages(int.Parse(userIdClaim.Value), secondUserId);

        return Ok(messages);
    }
    
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] MessagePostBindingModel messageDto)
    {
        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim is null) {
            throw new RestException(HttpStatusCode.NotFound, "Current user Id not found");
        }
        var senderId = int.Parse(userIdClaim.Value);
        await _messageService.SendMessage(senderId, messageDto.RecipientId, messageDto.Content);
        await _hubContext.Clients.User(messageDto.RecipientId.ToString()).SendAsync("ReceiveMessage", senderId, messageDto.Content);

        return Ok();
    }
}