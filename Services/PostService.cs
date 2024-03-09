using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Models;
using SocialNetwork.Models.ViewModels;

namespace SocialNetwork.Services;

public class PostService
{
    private readonly WorkSphereContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public PostService(WorkSphereContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<PostModel>> GetAllPostsAsync()
    {
        return await _context.Posts.Include(p => p.ApplicationUser).ToListAsync();
    }

    public async Task<PostModel> GetPostByIdAsync(int id)
    {
        return await _context.Posts.FindAsync(id);
    }

    public async Task CreatePostAsync(PostViewModel postViewModel, string userId)
    {
        PostModel postModel = CreatePostModelByViewModel(postViewModel, userId);
        _context.Add(postModel);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePostAsync(PostViewModel postViewModel, string userId)
    {
        var postModel = CreatePostModelByViewModel(postViewModel, userId);
        try
        {
            _context.Update(postModel);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return;
        }
    }

    public async Task DeletePostAsync(int id)
    {
        if (_context.Posts == null)
        {
            return;
        }

        var postModel = await GetPostByIdAsync(id);
        if (postModel != null)
        {
            _context.Posts.Remove(postModel);
            await _context.SaveChangesAsync();
        }
    }

    public PostModel CreatePostModelByViewModel(PostViewModel postViewModel, string userId)
    {
        return new PostModel
        {
            PostType = postViewModel.PostType,
            Title = postViewModel.Title,
            Description = postViewModel.Description,
            PostedOn = DateTime.Now,
            Category = postViewModel.Category,
            ApplicationUserId = userId,
        };
    }

    public PostViewModel CreateViewPostModelByModel(PostModel postModel)
    {
        return new PostViewModel
        {
            PostType = postModel.PostType,
            Title = postModel.Title,
            Description = postModel.Description,
            Category = postModel.Category
        };
    }
}