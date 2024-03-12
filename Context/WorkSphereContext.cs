using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;

namespace SocialNetwork.Context;

public class WorkSphereContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<PostModel> Posts { get; set; }
    public DbSet<ChatModel> Chats { get; set; }
    public DbSet<MessageModel> Messages { get; set; }
    public DbSet<ChatUser> ChatUsers { get; set; }

    public WorkSphereContext(DbContextOptions<WorkSphereContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChatUser>()
            .HasKey(cu => new { cu.UserId, cu.ChatId });

        modelBuilder.Entity<ChatUser>()
            .HasOne(cu => cu.User)
            .WithMany(u => u.ChatUsers)
            .HasForeignKey(cu => cu.UserId);

        modelBuilder.Entity<ChatUser>()
            .HasOne(cu => cu.Chat)
            .WithMany(c => c.ChatUsers)
            .HasForeignKey(cu => cu.ChatId);
    }
    
}