using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers;

public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly WorkSphereContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(ILogger<HomeController> logger, WorkSphereContext context,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        List<ApplicationUser> users = _userManager.Users.Include(u => u.Posts).ToList();
        return View(users);
    }

    public IActionResult Detail(string? id)
    {
        if (id == null)
        {
            return BadRequest("User ID cannot be null.");
        }

        ApplicationUser? user = _userManager.FindByIdAsync(id).Result;
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return View(user);
    }
}