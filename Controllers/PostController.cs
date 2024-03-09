using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Models;
using SocialNetwork.Models.ViewModels;

namespace SocialNetwork.Controllers
{
    public class PostController : Controller
    {
        private readonly WorkSphereContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(WorkSphereContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            return _context.Posts.Include(p => p.ApplicationUser).ToList() != null
                ? View(await _context.Posts.Include(p => p.ApplicationUser).ToListAsync())
                : Problem("Entity set 'WorkSphereContext.Posts'  is null.");
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel postViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    PostModel postModel = new PostModel
                    {
                        PostType = PostType.Looking,
                        Title = postViewModel.Title,
                        Description = postViewModel.Description,
                        PostedOn = DateTime.Now,
                        Category = postViewModel.Category,
                        ApplicationUserId = user.Id,
                    };
                    _context.Add(postModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Error when creating new post");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not logged in");
            }

            return View(postViewModel);
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts.FindAsync(id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,PostType,Title,Description,PostedOn,Category,ApplicationUserId")] PostModel postModel)
        {
            if (id != postModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostModelExists(postModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(postModel);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'WorkSphereContext.Posts'  is null.");
            }

            var postModel = await _context.Posts.FindAsync(id);
            if (postModel != null)
            {
                _context.Posts.Remove(postModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostModelExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}