namespace E2EChatApp.Core.Domain.Models;

public class Message
{
    public int ID { get; set; }

    public string SenderId { get; set; }

    public string ReceiverId { get; set; }

    public string Content { get; set; }

    public DateTime Timestamp { get; set; }
}