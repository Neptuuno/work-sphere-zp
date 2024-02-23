namespace SocialNetwork.Models;

public class PostModel
{
    public int Id { get; set; }
    public PostType PostType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime PostedOn { get; set; }
    public string Category { get; set; }
    public int UserId { get; set; }
}

public enum PostType
{
    Looking,
    Offering,
}