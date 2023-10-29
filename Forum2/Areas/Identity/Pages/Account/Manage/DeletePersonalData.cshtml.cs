// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Forum2.Controllers;
using Forum2.DAL;
using Forum2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;


namespace Forum2.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IForumThreadRepository _forumThreadRepository;
        private readonly IForumPostRepository _forumPostRepository;
        private readonly IWallPostRepository _wallPostRepository;
        private readonly IWallPostReplyRepository _wallPostReplyRepository;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IForumThreadRepository forumThreadRepository,
            IForumPostRepository forumPostRepository,
            IWallPostRepository wallPostRepository,
            IWallPostReplyRepository wallPostReplyRepository)
            
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _forumPostRepository = forumPostRepository;
            _forumThreadRepository = forumThreadRepository;
            _wallPostRepository = wallPostRepository;
            _wallPostReplyRepository = wallPostReplyRepository;
        }

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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }
            
            //Find all threads of user
            var threads = _forumThreadRepository.GetForumThreadsByAccountId(_userManager.GetUserAsync(User).Result.Id);
            //Find all posts of user
            var posts = _forumPostRepository.GetAllForumPostsByAccountId(_userManager.GetUserAsync(User).Result.Id);
            //Find all wall posts on users profile
            var wallPostsOnProfile = _wallPostRepository.GetAllByProfile(_userManager.GetUserAsync(User).Result.Id);
            //Find all wall posts created by user
            var wallPostsByCreator = _wallPostRepository.GetAllByCreator(_userManager.GetUserAsync(User).Result.Id);
            //Find all wall replies of user
            var wallPostReplies = _wallPostReplyRepository.GetAllByCreator(_userManager.GetUserAsync(User).Result.Id);
            
            //Delete
            foreach (var threadId in threads.Result)
            {
                await _forumThreadRepository.DeleteForumThread(threadId.Id);
            }
            
            foreach (var postId in posts.Result)
            {
                await _forumPostRepository.DeleteForumPost(postId.Id);
            }
            
            foreach (var postId in wallPostReplies.Result)
            {
                await _wallPostReplyRepository.Delete(postId.Id);
            }
            
            foreach (var postId in wallPostsByCreator.Result)
            {
                await _wallPostRepository.Delete(postId.Id);
            }
            
            foreach (var postId in wallPostsOnProfile.Result)
            {
                await _wallPostRepository.Delete(postId.Id);
            }
            
            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
