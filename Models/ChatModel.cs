namespace SocialNetwork.Models;

public class ChatModel
{
    public int Id { get; set; }
    public string? Color { get; set; } = "primary";
    public virtual ICollection<MessageModel> Messages { get; set; } = new List<MessageModel>();
    public virtual ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
}