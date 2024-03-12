namespace SocialNetwork.Models;

public class ChatModel
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public virtual MessageModel? Message { get; set; }
    public string? SenderId { get; set; }
    public virtual ApplicationUser? Sender { get; set; }
    public string? ReceiverId { get; set; }
    public virtual ApplicationUser? Receiver { get; set; }
}