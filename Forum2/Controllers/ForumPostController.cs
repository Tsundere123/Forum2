using Forum2.DAL;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum2.Controllers;

public class ForumPostController : Controller
{
    private readonly IForumCategoryRepository _forumCategoryRepository;
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ForumPostController(IForumCategoryRepository forumCategoryRepository, IForumThreadRepository forumThreadRepository,IForumPostRepository forumPostRepository, UserManager<ApplicationUser> userManager)
    {
        _forumCategoryRepository = forumCategoryRepository;
        _forumThreadRepository = forumThreadRepository;
        _forumPostRepository = forumPostRepository;
        _userManager = userManager;
    }

    private const int PageSize = 10;

    [HttpGet]
    [Route("/ForumPost/{forumThreadId}/{page?}")]
    public async Task<IActionResult> ForumPostView(int forumThreadId, int? page)
    {
        var forumPosts = await _forumPostRepository.GetAllForumPostsByThreadId(forumThreadId);
        var forumCategory = await _forumCategoryRepository.GetForumCategoryById(_forumThreadRepository.GetForumThreadById(forumThreadId).Result.ForumCategoryId);
        
        
        var currentForumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (currentForumThread.ForumThreadIsSoftDeleted)
        {
            if (!HttpContext.User.IsInRole("Moderator") && !HttpContext.User.IsInRole("Administrator"))
            {
                return NotFound();
            }
        }
        
        var forumPostsCount = forumPosts.Count();
        var totalPages = (int)Math.Ceiling((double)forumPostsCount / PageSize);
        var currentPage = page ?? 1;
        
        forumPosts = forumPosts.Skip((currentPage - 1) * PageSize).Take(PageSize);
        
        var accounts = await _userManager.Users.ToListAsync();
        var forumPostViewModel = new ForumPostViewModel
        {
            ForumCategory = forumCategory,
            CurrentForumThread = currentForumThread,
            ForumPosts = forumPosts,
            Accounts = accounts,
            CurrentPage = currentPage,
            TotalPages = totalPages
        };
        
        return View(forumPostViewModel);
    }
    [Authorize]
    [HttpGet]
    [Route("/ForumPost/Create/{forumThreadId}")]
    public async Task<IActionResult> CreateNewForumPost(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        var accounts = await _userManager.Users.ToListAsync();
        var forumPost = new ForumPost();
        forumPost.ForumThreadId = forumThread.ForumThreadId;
        var viewModel = new ForumPostCreationViewModel
        {
            ForumThread = forumThread,
            ForumPost = forumPost,
            Accounts = accounts
        };
        return PartialView(viewModel);
    }
    [Authorize]
    [HttpPost]
    [Route("/ForumPost/Create/{forumThreadId}")]
    public async Task<IActionResult> CreateNewForumPost(int forumThreadId, ForumPost forumPost)
    {
        ForumPost addPost = new ForumPost();
        addPost.ForumPostContent = forumPost.ForumPostContent;
        addPost.ForumThreadId = forumPost.ForumThreadId;
        addPost.ForumPostCreationTimeUnix = DateTime.UtcNow;
        addPost.ForumPostCreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
        await _forumPostRepository.CreateNewForumPost(addPost);
        
        // Get last page
        var forumPosts = await _forumPostRepository.GetAllForumPostsByThreadId(forumThreadId);
        var forumPostsCount = forumPosts.Count();
        var totalPages = (int)Math.Ceiling((double)forumPostsCount / PageSize);

        //Needed for RedirectToAction
        return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId, page = totalPages});
    }

    [Authorize]
    [HttpGet]
    [Route("/ForumPost/Update/{forumPostId}")]
    public async Task<IActionResult> UpdateForumPostContent(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.ForumPostCreatorId
            || HttpContext.User.IsInRole("Moderator") 
            || HttpContext.User.IsInRole("Administrator"))
        {
            if (forumPost == null) return NotFound();
            return View(forumPost);
        }
        return Forbid();
    }
    [Authorize]
    [HttpPost]
    [Route("/ForumPost/Update/{forumPostId}")]
    public async Task<IActionResult> UpdateForumPostContent(int forumPostId, ForumPost forumPost)
    {
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.ForumPostCreatorId
            || HttpContext.User.IsInRole("Moderator") 
            || HttpContext.User.IsInRole("Administrator"))
        {
            if (ModelState.IsValid)
            {
                forumPost.ForumPostLastEditedTime = DateTime.Now;
                forumPost.ForumPostLastEditedBy = _userManager.GetUserAsync(User).Result.Id;
                await _forumPostRepository.UpdateForumPost(forumPost);
                //Needed for RedirectToAction
                var forumThreadId = forumPost.ForumThreadId;
                return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId});
            }
            return View(forumPost);
        }
        return Forbid();
    }
    [Authorize]
    [HttpGet]
    [Route("/ForumPost/Delete/{forumPostId}")]
    public async Task<IActionResult> DeleteSelectedForumPost(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.ForumPostCreatorId
            || HttpContext.User.IsInRole("Moderator")
            || HttpContext.User.IsInRole("Administrator"))
        {
            if (forumPost == null) return NotFound();
            return View(forumPost);
        }

        return Forbid();
    }
    
    [Authorize(Roles = "Administrator,Moderator")]
    [HttpPost]
    public async Task<IActionResult> PermaDeleteSelectedForumPostConfirmed(int forumPostId)
    {
        //Needed for RedirectToAction
        var forumThreadId = _forumPostRepository.GetForumPostById(forumPostId).Result.ForumThreadId;
        await _forumPostRepository.DeleteForumPost(forumPostId);
        return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId});
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SoftDeleteSelectedForumPostContent(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.ForumPostCreatorId
            || HttpContext.User.IsInRole("Moderator") 
            || HttpContext.User.IsInRole("Administrator"))
        {
            //Needed for RedirectToAction
            var forumThreadId = forumPost.ForumThreadId;
            if (forumPost == null) return NotFound();
            // forumPost.ForumPostContent = "This post has been deleted";
            forumPost.ForumPostIsSoftDeleted = true;
            await UpdateForumPostContent(forumPostId,forumPost);
            return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId});
        }
        return Forbid();
    }
}
