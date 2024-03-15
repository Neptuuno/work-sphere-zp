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

    public UserService(UserManager<ApplicationUser> userManager, WorkSphereContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public List<ApplicationUser> GetAllUsers()
    {
        return _userManager.Users.Include(u => u.Posts).ToList();
    }

    public ApplicationUser? GetUserDetails(string id)
    {
        return _userManager.FindByIdAsync(id).Result;
    }

    public async Task<IdentityResult> EditUser(ApplicationUser user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult?> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            return await _userManager.DeleteAsync(user);
        }

        return null;
    }
    public Settings.InputModel GetUserSettings(string id)
    {
        var user = _userManager.FindByIdAsync(id).Result;
        if (user != null)
        {
            return new Settings.InputModel
            {
                Age = user.Age,
            };
        }

        return null;
    }
}