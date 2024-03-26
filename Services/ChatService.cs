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
            .Include(cu => cu.User)
            .Include(cu => cu.Chat)
            .Select(cu => cu.Chat)
            .ToListAsync();
        if (chats == null)
        {
            return null;
        }

        return chats;
    }

    public async Task SaveColor(ChatModel chat, string color)
    {
        chat.Color = color;
        await _context.SaveChangesAsync();
    }

   public async Task GetColor()
    {
        
        
    }

    public async Task<ChatModel> GetChatById(int id)
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

        if (currentUser == null || secondUser == null || currentUser == secondUser)
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
    
    public async Task<List<MessageModel>> GetMessagesForChat(ChatModel chat)
    {
        var messages = await _context.Chats
            .Where(c => c.Id == chat.Id)
            .Include(c => c.Messages)
            .ThenInclude(m => m.Sender)
            .SelectMany(c => c.Messages)
            .ToListAsync();

        return messages;
    }
    public async Task<MessageModel> CreateMessage(int chatId, string userId, string content)
    {
        var message = new MessageModel
        {
            ChatId = chatId,
            SenderId = userId,
            Content = content,
            Timestamp = DateTime.Now
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }
    
    public async Task<List<ApplicationUser>> GetOtherUsers(IEnumerable<ChatModel> chats, ApplicationUser user)
    {
        var otherUsers = new List<ApplicationUser>();
        foreach (var chat in chats)
        {
            var otherUser = await _context.ChatUsers
                .Where(cu => cu.ChatId == chat.Id && cu.UserId != user.Id)
                .Include(cu => cu.User)
                .Select(cu => cu.User)
                .FirstOrDefaultAsync();
            if (otherUser != null)
            {
                otherUsers.Add(otherUser);
            }
        }
        return otherUsers;
    }
    public async Task<ApplicationUser> GetOtherUser(ChatModel chat, ApplicationUser user)
    {
        var otherUser = await _context.ChatUsers
            .Where(cu => cu.ChatId == chat.Id && cu.UserId != user.Id)
            .Include(cu => cu.User)
            .Select(cu => cu.User)
            .FirstOrDefaultAsync();
        return otherUser;
    }
}