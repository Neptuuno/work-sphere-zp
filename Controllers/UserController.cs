using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Models;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers;

public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly WorkSphereContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly UserService _userService;

    public UserController(ILogger<HomeController> logger, WorkSphereContext context,
        UserManager<ApplicationUser> userManager, UserService userService)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View(_userService.GetAllUsers());
    }

    public IActionResult Detail(string? id)
    {
        if (id == null)
        {
            return BadRequest("User ID cannot be null.");
        }

        ApplicationUser? user = _userService.GetUserDetails(id);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return View(user);
    }
}