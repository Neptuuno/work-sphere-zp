using Humanizer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.Context;

public class WorkSphereContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<PostModel> Posts { get; set; }
    public DbSet<ChatModel> Chats { get; set; }
    public DbSet<MessageModel> Messages { get; set; }
    public DbSet<ContentModel> Contents { get; set; }

    public WorkSphereContext(DbContextOptions<WorkSphereContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.LikedPosts)
            .WithMany(p => p.LikedByUsers);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.ApplicationUser)
            .HasForeignKey(p => p.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Messages)
            .WithOne(m => m.Sender)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.SetNull);

    }
    
}