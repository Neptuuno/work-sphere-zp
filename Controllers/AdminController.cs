using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET
    public IActionResult Index()
    {
        List<ApplicationUser> users = _userManager.Users.Include(u => u.Posts).ToList();
        return View(users);
    }
    

    // POST: Admin/CreateRole
    // POST: RoleController/Create  
    [HttpPost]  
    [ValidateAntiForgeryToken]  
    public async Task<ActionResult> CreateRole(string roleName)  
    {  
        try  
        {  
            if (!string.IsNullOrEmpty(roleName))  
            {  
                if (!(await _roleManager.RoleExistsAsync(roleName)))  
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                    return RedirectToAction("Index");  
                } 
            }

            return RedirectToAction("Index");
        }  
        catch
        {
            return RedirectToAction("Index");
        }  
    }

    // POST: Admin/AssignRole
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (user != null && roleExists)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
        return RedirectToAction(nameof(Index));
    }
}