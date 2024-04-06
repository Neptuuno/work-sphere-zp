namespace SocialNetwork.Models;

public class ContentModel
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public string? ImageUrl { get; set; }
    public bool SpecialMessage { get; set; } = false;

    public int MessageId { get; set; }
    public MessageModel Message { get; set; }
}