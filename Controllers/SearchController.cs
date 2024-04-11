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
        var users = _context.Users
            .Select(u => new { Text = u.UserName, Type = "User" });

        var posts = _context.Posts
            .Select(p => new { Text = p.Title, Type = "Post" });

        var all = users.Concat(posts)
            .AsEnumerable()
            .Select(x => new { x.Text, x.Type, Score = Fuzz.Ratio(query, x.Text) })
            .Where(x => x.Score > 60) // Adjust the threshold as needed
            .OrderByDescending(x => x.Score)
            .Take(5)
            .ToList();

        return Ok(all);
    }
}