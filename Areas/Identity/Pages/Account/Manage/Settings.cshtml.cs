using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialNetwork.Models;
using SocialNetwork.Models.ViewModels;

namespace SocialNetwork.Areas.Identity.Pages.Account.Manage;

public class Settings : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public Settings(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }
    
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

        Input = new InputModel
        {
            Age = user.Age
        };
        
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

        user.Age = Input.Age;
        await _userManager.UpdateAsync(user);

        return RedirectToPage();
    }
}