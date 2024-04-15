using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SocialNetwork.Areas.Identity.Pages.Account.Manage;

public class TwoFactorAuthentication : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("/Account/Manage/Index");
    }
}