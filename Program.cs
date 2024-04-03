using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Context;
using SocialNetwork.Models;
using SocialNetwork.Context;
using SocialNetwork.Hubs;
using SocialNetwork.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WorkSphereContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WorkSphereContextConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<WorkSphereContext>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<FileService>();
// Add services to the container.
builder.Services.AddSignalR(hubOptions => { hubOptions.EnableDetailedErrors = true; });
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Index}/{userId?}");
app.MapRazorPages();
app.MapHub<ChatHub>("/chathub");
app.Run();
