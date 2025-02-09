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
using SocialNetwork.Models.InputModels;
using SocialNetwork.Models.ViewModels;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers;

[Authorize]
public class PostController : Controller
{
    private readonly WorkSphereContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly PostService _postService;

    public PostController(WorkSphereContext context, UserManager<ApplicationUser> userManager, PostService postService)
    {
        _context = context;
        _userManager = userManager;
        _postService = postService;
    }

    // GET: Post
    public async Task<IActionResult> Index()
    {
        return View(await _postService.GetAllPostsAsync());
    }

    // GET: Post/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Posts == null)
        {
            return StatusCode(404, "Not Found, Sorry!");
        }

        PostModel? postModel = await _postService.GetPostByIdAsync(id.Value);
        if (postModel == null)
        {
            return StatusCode(404, "Not Found, Sorry!");
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
    public async Task<IActionResult> Create(PostViewModel postViewModel, IFormFile? ImageUrl)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            if (ModelState.IsValid)
            {
                await _postService.CreatePostAsync(postViewModel, user.Id, ImageUrl);
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
        ApplicationUser? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest("User not logged in");
        }

        if (id == null || _context.Posts == null)
        {
            return NotFound();
        }

        var postModel = await _postService.GetPostByIdAsync(id.Value);
        if (postModel == null)
        {
            return NotFound();
        }

        if (!_postService.IsAuthor(user, postModel))
        {
            return BadRequest("Unauthorized");
        }

        var postViewModel = _postService.CreateViewPostModelByModel(postModel);

        return View(postViewModel);
    }

    // POST: Post/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PostViewModel postViewModel, IFormFile? ImageUrl)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest("User not logged in");
        }

        var postModel = await _postService.GetPostByIdAsync(id);

        if (!_postService.IsAuthor(user, postModel))
        {
            return BadRequest("Unauthorized");
        }

        if (ModelState.IsValid)
        {
            await _postService.UpdatePostAsync(postModel, postViewModel, user.Id, ImageUrl);
            return RedirectToAction(nameof(Details), new { id = id });
        }

        return View(postViewModel);
    }

    [HttpGet]
    public async Task<uint> GetLikesCount(int id)
    {
        return await _postService.GetPostLikesCount(id);
    }

    [HttpPost]
    public async Task UpdateLike([FromBody] UpdateLikeInputModel input)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("like changed");
        Console.WriteLine(input.Id);
        Console.WriteLine(input.Liked);
        ApplicationUser? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return;
        }

        var postModel = await _postService.GetPostByIdAsync(input.Id);
        if (postModel == null)
        {
            return;
        }

        await _postService.UpdateLike(postModel, user, input.Liked);
    }

    // POST: Post/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest("User not logged in");
        }

        var postModel = await _postService.GetPostByIdAsync(id);
        if (!_postService.IsAuthorized(user, postModel))
        {
            return BadRequest("Unauthorized");
        }

        await _postService.DeletePostAsync(id);
        return RedirectToAction(nameof(Index));
    }
}