using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Models;

namespace SocialNetwork.Services;

public class ChatService
{
    private readonly WorkSphereContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ChatService(WorkSphereContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IEnumerable<ChatModel>> GetChatsForUser(ApplicationUser user)
    {
        var chatUsers = user.ChatUsers;
        var chats = new List<ChatModel>();
        foreach (var chatUser in chatUsers)
        {
            var chat = chatUser.Chat;
            if (chat != null)
            {
                chats.Add(chat);
            }
        }

        return chats;
    }

    public async Task<ChatModel> GetChatById(ApplicationUser user, int id)
    {
        var chatUsers = user.ChatUsers;
        var chat = chatUsers.First(c => c.ChatId == id).Chat;
        if (chat == null)
        {
            throw new Exception("Invalid id");
        }

        if (chat.ChatUsers.Contains(chatUsers.First(c => c.UserId == user.Id)))
        {
            return chat;
        }

        throw new Exception("User is not in chat");
    }

    public async Task<ChatModel> CreateChat(string currentUserID, string secondUserID)
    {
        var currentUser = await _userManager.FindByIdAsync(currentUserID);
        var secondUser = await _userManager.FindByIdAsync(secondUserID);

        if (currentUser == null || secondUser == null)
        {
            return null;
        }

        var existingChat = _context.Chats
            .Include(c => c.ChatUsers)
            .FirstOrDefault(c =>
                c.ChatUsers.Any(cu => cu.UserId == currentUserID) && c.ChatUsers.Any(cu => cu.UserId == secondUserID));

        if (existingChat != null)
        {
            return null;
        }
        
        var chat = new ChatModel
        {
            ChatUsers = new List<ChatUser>
            {
                new ChatUser {User = currentUser},
                new ChatUser {User = secondUser}
            }
        };
        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();

        return chat;
    }
}