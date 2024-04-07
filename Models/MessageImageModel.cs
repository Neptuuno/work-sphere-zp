using Newtonsoft.Json;

namespace SocialNetwork.Models;

public class MessageImageModel
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    
    [JsonIgnore]
    public ContentModel? Content { get; set; }
}