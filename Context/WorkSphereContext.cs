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
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<ApplicationUser>()
    //         .HasMany(e => e.Posts)
    //         .WithOne(e => e.ApplicationUser)
    //         .HasForeignKey(e => e.ApplicationUserId)
    //         .IsRequired();
    // }
    
}