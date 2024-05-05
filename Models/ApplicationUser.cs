using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Models;

public class ApplicationUser : IdentityUser
{
    public string? Bio { get; set; }
    public string? ImageUrl { get; set; }
    public int? LastOpenedChatId { get; set; }
    public ICollection<PostModel> Posts { get; set; } = new List<PostModel>();
    public ICollection<ChatModel> Chats { get; set; } = new List<ChatModel>();
    public ICollection<MessageModel> Messages { get; set; } = new List<MessageModel>();
    public ICollection<PostModel> LikedPosts { get; set; } = new List<PostModel>();
}