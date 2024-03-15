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
    private readonly FileService _fileService;

    public PostService(WorkSphereContext context, UserManager<ApplicationUser> userManager, FileService fileService)
    {
        _context = context;
        _userManager = userManager;
        _fileService = fileService;
    }

    public async Task<List<PostModel>> GetAllPostsAsync()
    {
       return  await _context.Posts.Include(p => p.ApplicationUser).ToListAsync();
    }
    
    public async Task<List<PostModel>> GetAllPostsAsyncSortedByIndex(string userId)
    {
        return  await _context.Posts.Include(p => p.ApplicationUser).ToListAsync();
    }

    public async Task<PostModel> GetPostByIdAsync(int id)
    {
        return await _context.Posts.FindAsync(id);
    }

    public async Task CreatePostAsync(PostViewModel postViewModel, string userId, IFormFile? image)
    {
        PostModel postModel = CreatePostModelByViewModel(postViewModel, userId);
        if (image != null)
        {
            postModel.ImageUrl = await _fileService.SaveImageAsync(image, userId, "posts");
        }
        _context.Add(postModel);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePostAsync(PostModel postModel, PostViewModel postViewModel, string userId, IFormFile? image)
    {
        var updatedPostModel = UpdatePostModelByViewModel(postModel, postViewModel, userId);
        if (image != null)
        {
            string? existingFileName = Path.GetFileName(postModel.ImageUrl);
            updatedPostModel.ImageUrl = await _fileService.SaveImageAsync(image, userId, "posts", existingFileName);

        }
        try
        {
            _context.Update(updatedPostModel);
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

    public bool IsAuthorized(ApplicationUser user, PostModel post)
    {
        return post.ApplicationUserId == user.Id;
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
    public PostModel UpdatePostModelByViewModel(PostModel postModel, PostViewModel postViewModel, string userId)
    {
        return new PostModel
        {
            PostType = postViewModel.PostType,
            Title = postViewModel.Title,
            Description = postViewModel.Description,
            PostedOn = postModel.PostedOn,
            UpdatedOn = DateTime.Now,
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