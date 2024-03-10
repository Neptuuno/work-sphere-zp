namespace SocialNetwork.Models;

public class PostModel
{
    public int Id { get; set; }
    public PostType PostType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    // public int Likes { get; set; } = 0;

    public DateTime PostedOn { get; set; }
     public DateTime? UpdatedOn { get; set; }
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