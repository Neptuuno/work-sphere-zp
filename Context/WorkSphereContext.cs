using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.Context;

public class WorkSphereContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<PostModel>? Posts { get; set; }
    public WorkSphereContext(DbContextOptions<WorkSphereContext> options)
        : base(options)
    {
    }
    
}