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
    private readonly ChatService _chatService;


    public UserService(UserManager<ApplicationUser> userManager, WorkSphereContext context, FileService fileService,
        ChatService chatService)
    {
        _userManager = userManager;
        _context = context;
        _fileService = fileService;
        _chatService = chatService;
    }

    public List<ApplicationUser> GetAllUsers()
    {
        return _userManager.Users.Include(u => u.Posts).ToList();
    }

    public async Task<ApplicationUser?> GetUserDetails(string id)
    {
        return await _userManager.Users.Where(u => u.Id == id)
            .Include(u => u.Posts)
            .FirstOrDefaultAsync();
    }

    public async Task<ApplicationUser?> GetSafeUserDetails(string id)
    {
        var userDetail = await _userManager.FindByIdAsync(id);
        if (userDetail == null) throw new Exception("User not found");
        return new ApplicationUser
        {
            Id = userDetail.Id,
            UserName = userDetail.UserName,
            ImageUrl = userDetail.ImageUrl,
        };
    }

    public int? GetLastOpenedChatIdForUser(ApplicationUser user)
    {
        return user.LastOpenedChatId;
    }

    public void SetLastOpenedChatIdForUser(ApplicationUser user, int chatId)
    {
        user.LastOpenedChatId = chatId;
        _userManager.UpdateAsync(user);
    }

    public async Task SetUserImage(ApplicationUser user, IFormFile? image)
    {
        if (image != null)
        {
            string? existingFileName = Path.GetFileName(user.ImageUrl);
            user.ImageUrl = await _fileService.SaveImageAsync(image, user.Id, "users", existingFileName);
        }
    }

    public async Task<bool> CanRemoveUser(ApplicationUser? user, ApplicationUser? toRemove)
    {
        if (user == null || toRemove == null || user.Id == toRemove.Id) return false;

        var userRoles = await _userManager.GetRolesAsync(user);
        var toRemoveRoles = await _userManager.GetRolesAsync(toRemove);

        if (userRoles.Contains("SuperAdmin") && !toRemoveRoles.Contains("SuperAdmin")) return true;
        if (userRoles.Contains("Admin") && !toRemoveRoles.Contains("Admin") &&
            !toRemoveRoles.Contains("SuperAdmin")) return true;

        return false;
    }

    public ApplicationUser GetUserModel(IndexModel.InputModel inputModel, ApplicationUser user)
    {
        user.Age = inputModel.Age;
        return user;
    }
}