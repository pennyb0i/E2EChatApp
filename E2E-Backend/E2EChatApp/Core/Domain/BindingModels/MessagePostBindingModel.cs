namespace E2EChatApp.Core.Domain.BindingModels;

public class MessagePostBindingModel {
    public int RecipientId { get; set; }
    public required string Content { get; set; }
}
