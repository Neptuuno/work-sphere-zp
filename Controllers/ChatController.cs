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
using SocialNetwork.Services;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly WorkSphereContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ChatService _chatService;

        public ChatController(WorkSphereContext context, UserManager<ApplicationUser> userManager,
            ChatService chatService)
        {
            _context = context;
            _userManager = userManager;
            _chatService = chatService;
        }

        // GET: Chat
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var chats = await _chatService.GetChatsForUser(user);
            return View(chats);
        }

        // GET: Chats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }

            var chat = await _chatService.GetChatById(user, id.Value);

            return View(chat);
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
            return RedirectToAction(nameof(Index));
        }

        private bool ChatModelExists(int id)
        {
            return (_context.Chats?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}