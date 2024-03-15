using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Areas.Identity.Pages.Account.Manage;
using SocialNetwork.Context;
using SocialNetwork.Models;

namespace SocialNetwork.Services;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly WorkSphereContext _context;
    private readonly FileService _fileService;


    public UserService(UserManager<ApplicationUser> userManager, WorkSphereContext context, FileService fileService)
    {
        _userManager = userManager;
        _context = context;
        _fileService = fileService;
    }

    public List<ApplicationUser> GetAllUsers()
    {
        return _userManager.Users.Include(u => u.Posts).ToList();
    }

    public ApplicationUser? GetUserDetails(string id)
    {
        return _userManager.FindByIdAsync(id).Result;
    }

    public async Task UpdateUser(ApplicationUser user ,Settings.InputModel newUserModel, IFormFile? image)
    {
        await SetUserImage(user, image);
        await _userManager.UpdateAsync(GetUserModel(newUserModel, user));
    }
    public async Task SetUserImage( ApplicationUser user, IFormFile? image)
    {
        if (image != null)
        {
            string? existingFileName = Path.GetFileName(user.ImageUrl);
            user.ImageUrl = await _fileService.SaveImageAsync(image, user.Id, "users", existingFileName);
        }
    }
    
    public Settings.InputModel GetInputModel(ApplicationUser user)
    {
        return new Settings.InputModel
        {
            Age = user.Age,
            ImageUrl = user.ImageUrl,
        };
    }

    private ApplicationUser GetUserModel(Settings.InputModel inputModel, ApplicationUser user)
    {
        user.Age = inputModel.Age;
        return user;
    }
}