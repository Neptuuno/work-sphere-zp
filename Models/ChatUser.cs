namespace SocialNetwork.Models;

public class ChatUser
{
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public int? ChatId { get; set; }
    public ChatModel? Chat { get; set; }
}