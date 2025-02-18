using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly WorkSphereContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UserService _userService;

    public UserController(WorkSphereContext context,
        UserManager<ApplicationUser> userManager, UserService userService)
    {
        _context = context;
        _userManager = userManager;
        _userService = userService;
    }

    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return BadRequest("User ID cannot be null.");
        }

        ApplicationUser? user = await _userService.GetUserDetails(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return View(user);
    }
}