using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialNetwork.Services;
using SocialNetwork.Models;

namespace SocialNetwork.Areas.Identity.Pages.Account.Manage
{
    public class Posts : PageModel
    {
        private readonly PostService _postService;
        private readonly UserManager<ApplicationUser> _userManager;

        public Posts(PostService postService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _userManager = userManager;
        }

        public IEnumerable<PostModel> UserPosts { get; set; } = new List<PostModel>();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            UserPosts = await _postService.GetPostsByUserAsync(user?.Id);
        }
    }
}