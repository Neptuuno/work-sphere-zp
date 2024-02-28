using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Models;

public class ApplicationUser: IdentityUser
{
    public int Age { get; set; }
    public ICollection<PostModel> Posts { get; set; } = new List<PostModel>();
}