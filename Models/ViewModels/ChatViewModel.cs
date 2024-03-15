namespace SocialNetwork.Models.ViewModels;

public class ChatViewModel
{
    public int Id { get; set; }
    public ICollection<MessageModel> Messages { get; set; } = new List<MessageModel>();
    public ApplicationUser? UserSelf { get; set; }
    public ApplicationUser? UserOther { get; set; }
}