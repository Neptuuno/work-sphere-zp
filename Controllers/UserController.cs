using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Context;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers;

public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public UserController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<ApplicationUser> users = new List<ApplicationUser>();
        using (var db = new WorkSphereContext())
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
        using (var db = new WorkSphereContext())
        {
            db.Add(applicationUser);
            db.SaveChanges();
        }

        return View();
    }
}