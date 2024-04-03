using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers;

[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserService _userService;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, UserService userService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userService = userService;
    }

    // GET
    public IActionResult Index()
    {
        List<ApplicationUser> users = _userManager.Users.Include(u => u.Posts).ToList();
        var roles = _roleManager.Roles.ToList();
        ViewBag.Roles = roles;
        return View(users);
    }

    // POST: Admin/CreateRole
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SuperAdmin")]
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
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> AssignRole(string id, string roleName)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (await _userManager.IsInRoleAsync(currentUser, "SuperAdmin"))
        {
            var user = await _userManager.FindByIdAsync(id);
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            var isCurrentUser = currentUser == user;
            if (user != null && roleExists && roleName != "SuperAdmin" && !isCurrentUser)
            {
                var oldRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, oldRoles);
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/DeleteRole
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role != null)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            if (!(usersInRole.Count > 0))
            {
                await _roleManager.DeleteAsync(role);
            }
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/DeleteUser
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var user = await _userManager.FindByIdAsync(userId);
        if (await _userService.CanRemoveUser(currentUser, user))
        {
            await _userManager.DeleteAsync(user);
        }

        return RedirectToAction(nameof(Index));
    }
}