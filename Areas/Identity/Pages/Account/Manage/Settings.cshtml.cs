using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialNetwork.Models;
using SocialNetwork.Models.ViewModels;
using SocialNetwork.Services;

namespace SocialNetwork.Areas.Identity.Pages.Account.Manage;

public class Settings : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UserService _userService;

    public Settings(UserManager<ApplicationUser> userManager, UserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    [BindProperty] 
    public IFormFile Image { get; set; }
    
    public class InputModel : ApplicationUserViewModel
    {
        
    }
    
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        Input = _userService.GetInputModel(user);
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _userService.UpdateUser(user,Input, Image);

        return RedirectToPage();
    }
}