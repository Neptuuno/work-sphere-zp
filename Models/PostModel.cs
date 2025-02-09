using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models;

public class PostModel
{
    public int Id { get; set; }
    public PostType PostType { get; set; }
    [MaxLength(32)]
    public string Title { get; set; }
    public string Description { get; set; }
     
    [DisplayName("Image")]
    public string? ImageUrl { get; set; }
    public DateTime PostedOn { get; set; }
     public DateTime? UpdatedOn { get; set; }
     [MaxLength(24)]
    public string Category { get; set; }

    // public List<CommentModel> Comments { get; set; }
    
    public ICollection<ApplicationUser> LikedByUsers { get; set; } = new List<ApplicationUser>();

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}

public enum PostType
{
    Looking,
    Offering,
}

public enum Category
{
    
}