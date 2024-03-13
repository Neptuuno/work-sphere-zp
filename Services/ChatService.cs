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

    public async Task<List<ChatModel?>> GetChatsForUser(ApplicationUser user)
    {
        var chats = await _context.ChatUsers
            .Where(cu => cu.UserId == user.Id)
            .Include(cu => cu.Chat)
            .Select(cu => cu.Chat)
            .ToListAsync();
        if (chats == null)
        {
            return null;
        }

        return chats;
    }

    public async Task<ChatModel> GetChatById(ApplicationUser user, int id)
    {
        var chat = await _context.Chats
            .Where(c => c.Id == id)
            .Include(c => c.ChatUsers)
            .ThenInclude(cu => cu.User)
            .FirstOrDefaultAsync();
        if (chat == null)
        {
            throw new Exception("Invalid id");
        }

        return chat;
    }

    public async Task<ChatModel> CreateChat(string currentUserID, string otherUserID)
    {
        var currentUser = await _userManager.FindByIdAsync(currentUserID);
        var secondUser = await _userManager.FindByIdAsync(otherUserID);

        if (currentUser == null || secondUser == null)
        {
            return null;
        }

        var existingChat = _context.Chats
            .Include(c => c.ChatUsers)
            .FirstOrDefault(c =>
                c.ChatUsers.Any(cu => cu.UserId == currentUserID) && c.ChatUsers.Any(cu => cu.UserId == otherUserID));

        if (existingChat != null)
        {
            return null;
        }

        var chat = new ChatModel
        {
            ChatUsers = new List<ChatUser>
            {
                new ChatUser { User = currentUser },
                new ChatUser { User = secondUser }
            }
        };
        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();

        return chat;
    }
}