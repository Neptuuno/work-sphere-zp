using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Models.ViewModels;

public class PostViewModel
{
    public PostType PostType { get; set; }
    [MaxLength(16)]
    public string Title { get; set; }
    public string Description { get; set; }
    [DisplayName("Image")]
    public string? ImageUrl { get; set; }
    [MaxLength(16)]

    public string Category { get; set; }
}