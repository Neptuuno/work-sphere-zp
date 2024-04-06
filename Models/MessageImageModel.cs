namespace SocialNetwork.Models;

public class MessageImageModel
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    
    public ContentModel? Content { get; set; }
}