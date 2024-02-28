using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Context;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers;

public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly WorkSphereContext _context;

    public UserController(ILogger<HomeController> logger, WorkSphereContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        List<ApplicationUser> users;
        using (var db = _context)
        {
            users = db.Users.ToList();
        }
        return View(users);
    }
}