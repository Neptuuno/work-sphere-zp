using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Models;

public class ApplicationUser : IdentityUser
{
    public int? Age { get; set; }
    public string? ImageUrl { get; set; }
    public int? LastOpenedChatId { get; set; }
    public ICollection<PostModel> Posts { get; set; } = new List<PostModel>();
    public ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();
}