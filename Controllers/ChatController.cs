using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Models;
using SocialNetwork.Models.ViewModels;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly WorkSphereContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ChatService _chatService;
        private readonly UserService _userService;

        public ChatController(WorkSphereContext context, UserManager<ApplicationUser> userManager,
            ChatService chatService, UserService userService)
        {
            _context = context;
            _userManager = userManager;
            _chatService = chatService;
            _userService = userService;
        }

        // GET: Chat
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var lastChatId = _userService.GetLastOpenedChatIdForUser(user);
            return RedirectToAction(nameof(Details), new { id = lastChatId });
        }

        // GET: Chats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var chat = id.HasValue
                ? await _chatService.GetChatById(id.Value)
                : _chatService.GetChatsForUser(user).Result.FirstOrDefault();

            if (chat == null || chat.Users.All(u => u.Id != user.Id))
            {
                return View("Index");
            }

            var chatViewModel = new ChatViewModel
            {
                Id = chat.Id,
                UserSelf = user,
                UserOther = await _chatService.GetOtherUser(chat, user),
                Messages = await _chatService.GetMessagesForChat(chat),
                Color = chat.Color
            };

            _userService.SetLastOpenedChatIdForUser(user, chat.Id);
            return View(chatViewModel);
        }

        //POST: Chat/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string secondUserId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            await _chatService.CreateChat(currentUser.Id, secondUserId);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateColor(int chatId, string color)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            
            var chat = await _chatService.GetChatById(chatId);

            if (chat == null || chat.Users.All(u => u.Id != user.Id))
            {
                return NotFound();
            }

            await _chatService.SaveColor(chat, color);

            return RedirectToAction(nameof(Details), new { id = chat.Id });
        }

        // POST: Chat/Delete/5 TODO(Do something later maybe?)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chats == null)
            {
                return Problem("Entity set 'WorkSphereContext.Chats'  is null.");
            }

            var chatModel = await _context.Chats.FindAsync(id);
            if (chatModel != null)
            {
                _context.Chats.Remove(chatModel);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool ChatModelExists(int id)
        {
            return (_context.Chats?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}