using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.Context;

public class WorkSphereContext : DbContext
{
    public DbSet<ApplicationUser>? Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=D:\\Adam\\OneDrive\\OneDrive - Smíchovská střední průmyslová škola\\PVA\\Zaverecny-Projekt\\SocialNetwork/Database/WorkSphere.db");
    }
}