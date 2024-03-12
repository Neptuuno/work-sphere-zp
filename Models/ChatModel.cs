namespace SocialNetwork.Models;

public class ChatModel
{
    public int Id { get; set; }
    public virtual ICollection<MessageModel>? Messages { get; set; }
    public virtual ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
}