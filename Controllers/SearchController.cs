using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Context;

namespace SocialNetwork.Controllers;

[Authorize]
public class SearchController : Controller
{
    private readonly WorkSphereContext _context;

    public SearchController(WorkSphereContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get(string query)
    {
        query = query.ToLower();

        var users = _context.Users
            .Where(u => u.UserName != null && u.UserName.ToLower().Contains(query))
            .Select(u => new { Id = u.Id, Text = u.UserName, ImageUrl = u.ImageUrl, Type = "User" });

        var posts = _context.Posts
            .Where(p => p.Title.ToLower().Contains(query))
            .Select(p => new {Id = p.Id.ToString(), Text = p.Title, ImageUrl = p.ImageUrl, Type = "Post" });

        var all = users.Concat(posts)
            .OrderByDescending(x => x.Text.ToLower().Contains(query))
            .Take(5)
            .ToList();

        return Ok(all);
    }
}