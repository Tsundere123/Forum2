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
        var currentForumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (currentForumThread.IsSoftDeleted)
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
        var forumPostViewModel = new ForumPostViewModel
        {
            CurrentForumThread = currentForumThread,
            ForumPosts = forumPosts,
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
        if (forumThread.IsLocked) return BadRequest();
        
        var forumPost = new ForumPost();
        forumPost.ThreadId = forumThread.Id;
        var viewModel = new ForumPostCreationViewModel
        {
            ForumThread = forumThread,
            ForumPost = forumPost
        };
        return PartialView(viewModel);
    }
    [Authorize]
    [HttpPost]
    [Route("/ForumPost/Create/{forumThreadId}")]
    public async Task<IActionResult> CreateNewForumPost(int forumThreadId, ForumPost forumPost)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (forumThread.IsLocked) return BadRequest();
        
        ForumPost addPost = new ForumPost();
        addPost.Content = forumPost.Content;
        addPost.ThreadId = forumPost.ThreadId;
        addPost.CreatedAt = DateTime.UtcNow;
        addPost.CreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
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
        if (forumPost.Thread.IsLocked) return BadRequest();
        
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.CreatorId
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
        if (forumPost.Thread.IsLocked) return BadRequest();
        
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.CreatorId
            || HttpContext.User.IsInRole("Moderator") 
            || HttpContext.User.IsInRole("Administrator"))
        {
            if (ModelState.IsValid)
            {
                forumPost.EditedAt = DateTime.Now;
                forumPost.EditedBy = _userManager.GetUserAsync(User).Result.Id;
                await _forumPostRepository.UpdateForumPost(forumPost);
                //Needed for RedirectToAction
                var forumThreadId = forumPost.ThreadId;
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
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.CreatorId
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
        var forumThreadId = _forumPostRepository.GetForumPostById(forumPostId).Result.ThreadId;
        await _forumPostRepository.DeleteForumPost(forumPostId);
        return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId});
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SoftDeleteSelectedForumPostContent(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.CreatorId
            || HttpContext.User.IsInRole("Moderator") 
            || HttpContext.User.IsInRole("Administrator"))
        {
            //Needed for RedirectToAction
            var forumThreadId = forumPost.ThreadId;
            if (forumPost == null) return NotFound();
            // forumPost.ForumPostContent = "This post has been deleted";
            forumPost.IsSoftDeleted = true;
            await UpdateForumPostContent(forumPostId,forumPost);
            return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId});
        }
        return Forbid();
    }
}
