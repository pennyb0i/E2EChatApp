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
    [HttpGet("{user1Id}/{user2Id}")]
    public async Task<IActionResult> Get(string user1Id, string user2Id)
    {
        var messages = await _messageService.GetMessages(user1Id, user2Id);

        return Ok(messages);
    }
    
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
    {
        Console.WriteLine("SENDING MESSEAGE with content");
        Console.WriteLine(messageDto.Content);
        await _hubContext.Clients.User(messageDto.ReceiverId).SendAsync("ReceiveMessage", messageDto.SenderId, messageDto.Content);

        return Ok();
    }
}