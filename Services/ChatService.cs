using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Models;

namespace SocialNetwork.Services
{
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
            var chats = await _context.Chats
                .Where(c => c.Users.Contains(user))
                .Include(c => c.Users)
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

        public string? GetColor(ChatModel chat)
        {
            return chat.Color;
        }

        public async Task<ChatModel> GetChatById(int id)
        {
            var chat = await _context.Chats
                .Where(c => c.Id == id)
                .Include(c => c.Users)
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
                .Include(c => c.Users)
                .FirstOrDefault(c =>
                    c.Users.Any(u => u.Id == currentUserID) && c.Users.Any(u => u.Id == otherUserID));

            if (existingChat != null)
            {
                return null;
            }

            var chat = new ChatModel
            {
                Users = new List<ApplicationUser> { currentUser, secondUser }
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

        public async Task<MessageModel> CreateMessage(int chatId, string userId, ContentModel content)
        {
            var message = new MessageModel
            {
                ChatId = chatId,
                SenderId = userId,
                Content = content,
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }
        
        public async Task DeleteMessage(int messageId)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            if (message == null)
            {
                return;
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }

        public string GetTimestampFormat(DateTime timestamp)
        {
            DateTime currentDate = DateTime.Now;
            DateTime inputDate = timestamp;

            if (currentDate.Year == inputDate.Year)
            {
                if (currentDate.Date == inputDate.Date)
                {
                    return inputDate.ToString("HH:mm");
                }

                return inputDate.ToString("dd/MM");
            }

            return inputDate.ToString("dd/MM/yyyy");
        }

        public List<ApplicationUser> GetOtherUsers(IEnumerable<ChatModel> chats, ApplicationUser user)
        {
            var otherUsers = new List<ApplicationUser>();
            foreach (var chat in chats)
            {
                var otherUser = chat.Users.FirstOrDefault(u => u.Id != user.Id);
                if (otherUser != null)
                {
                    otherUsers.Add(otherUser);
                }
            }

            return otherUsers;
        }

        public ApplicationUser? GetOtherUser(ChatModel? chat, ApplicationUser user)
        {
            var otherUser = chat?.Users.FirstOrDefault(u => u.Id != user.Id);
            return otherUser;
        }
    }
}