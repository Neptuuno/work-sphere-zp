namespace SocialNetwork.Models;

public class ChatUser
{
    public string? UserId { get; set; }
    public virtual ApplicationUser? User { get; set; }

    public int ChatId { get; set; }
    public virtual ChatModel? Chat { get; set; }
}