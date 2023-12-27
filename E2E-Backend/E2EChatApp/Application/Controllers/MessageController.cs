using E2EChatApp.Core.Domain.Dtos;
using E2EChatApp.Core.Domain.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace E2EChatApp.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
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