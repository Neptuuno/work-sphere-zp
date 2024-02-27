using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.Context;

public class WorkSphereContext : IdentityDbContext<ApplicationUser>
{
    public WorkSphereContext(DbContextOptions<WorkSphereContext> options)
        : base(options)
    {
    }

    //delete later :]
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlite("Data Source=/home/neptuno/Programming/c#/pva/zaverecna-prace/Database/WorkSphere.db");
    // }
}