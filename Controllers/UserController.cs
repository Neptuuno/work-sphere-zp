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
        List<ApplicationUser> users = new List<ApplicationUser>();
        using (var db = _context)
        {
            users = db.Users.ToList();
        }
        return View(users);
    }

    public IActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddUser(ApplicationUser applicationUser)
    {
        using (var db = _context)
        {
            db.Add(applicationUser);
            db.SaveChanges();
        }

        return View();
    }
}