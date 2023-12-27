namespace E2EChatApp.Core.Domain.Dtos;

public class MessageDto
{
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public string Content { get; set; }
}