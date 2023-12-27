using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.Dtos;
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
    [HttpGet("{firstUserId}/{secondUserId}")]
    public async Task<IActionResult> Get(int firstUserId, int secondUserId)
    {
        var messages = await _messageService.GetMessages(firstUserId, secondUserId);

        return Ok(messages);
    }
    
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
    {
        await _messageService.SendMessage(messageDto.SenderId, messageDto.ReceiverId, messageDto.Content);
        await _hubContext.Clients.User(messageDto.ReceiverId.ToString()).SendAsync("ReceiveMessage", messageDto.SenderId, messageDto.Content);

        return Ok();
    }
}