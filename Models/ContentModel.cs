using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models
{
    public class ContentModel
    {
        public int Id { get; set; }
        [MaxLength(20000)] public string? Text { get; set; }

        public ICollection<MessageImageModel>? Images { get; set; }

        public bool SpecialMessage { get; set; } = false;

        public int MessageId { get; set; }
        public MessageModel Message { get; set; }
    }
}