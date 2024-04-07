using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SocialNetwork.Models
{
    public class ContentModel
    {
        public int Id { get; set; }
        [MaxLength(20000)] public string? Text { get; set; }

        public ICollection<MessageImageModel>? Images { get; set; }

        public bool SpecialMessage { get; set; } = false;

        public int MessageId { get; set; }
        
        [JsonIgnore]
        public MessageModel Message { get; set; }
    }
}