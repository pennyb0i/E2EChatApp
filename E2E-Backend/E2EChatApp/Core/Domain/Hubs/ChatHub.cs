using Microsoft.AspNetCore.SignalR;

namespace E2EChatApp.Core.Domain.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string senderId, string receiverId, string content)
    {
        if (Clients.User(receiverId) != null)
        {
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, content);
        }
    }

    public async Task SendMessageAll(string message)
    {
        await Clients.All.SendAsync("ReceiveMessageAll", $"{Context.ConnectionId} {message}");
    }
    
    public override async Task OnConnectedAsync ()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
    }
}