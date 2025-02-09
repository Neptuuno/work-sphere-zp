// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SocialNetwork.Models;
using SocialNetwork.Models.ViewModels;
using SocialNetwork.Services;

namespace SocialNetwork.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserService _userService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, UserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel : ApplicationUserViewModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

            [Required]
            [StringLength(32, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
            [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "The {0} can only contain alphanumeric characters.")]
            [Display(Name = "User name")]
            public string UserName { get; set; }
        }

        [BindProperty] public IFormFile Image { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);


            Input = new InputModel
            {
                UserName = userName,
                Bio = user.Bio,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Update the user's image and other details regardless of the username
            await _userService.SetUserImage(user, Image);
            await _userManager.UpdateAsync(_userService.GetUserModel(Input, user));

            // Only attempt to update the username if it is valid and not taken
            if (ModelState.IsValid && user.UserName != Input.UserName && await _userManager.FindByNameAsync(Input.UserName) == null)
            {
                await _userManager.SetUserNameAsync(user, Input.UserName);
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your username has been updated";
            }
            else if (user.UserName != Input.UserName)
            {
                // If the username is invalid or taken, add an error to the ModelState
                ModelState.AddModelError("Input.UserName", "This username is already taken.");
            }

            // Reload the page to display the updated profile and any validation errors
            await LoadAsync(user);
            return Page();
        }
    }
}