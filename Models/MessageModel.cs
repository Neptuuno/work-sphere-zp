namespace SocialNetwork.Models;

public class MessageModel
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public DateTime Timestamp { get; set; }

    public string? SenderId { get; set; }
    public ApplicationUser? Sender { get; set; }

    public int ChatId { get; set; }
    public ChatModel Chat { get; set; }
}