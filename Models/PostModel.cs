using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models;

public class PostModel
{
    public int Id { get; set; }
    public PostType PostType { get; set; }
    [MaxLength(16)]
    public string Title { get; set; }
    public string Description { get; set; }
    // public int Likes { get; set; } = 0;
    [DisplayName("Image")]
    public string? ImageUrl { get; set; }
    public DateTime PostedOn { get; set; }
     public DateTime? UpdatedOn { get; set; }
     [MaxLength(16)]
    public string Category { get; set; }

    // public List<CommentModel> Comments { get; set; }
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