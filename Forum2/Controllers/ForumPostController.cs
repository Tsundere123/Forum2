using Forum2.DAL;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Forum2.Controllers;

public class ForumPostController : Controller
{
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ForumPostController(IForumThreadRepository forumThreadRepository,IForumPostRepository forumPostRepository, UserManager<ApplicationUser> userManager)
    {
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
        
        if (currentForumThread == null || forumPosts == null) return NotFound();
        
        if (currentForumThread.IsSoftDeleted)
        {
            if (!HttpContext.User.IsInRole("Moderator") && !HttpContext.User.IsInRole("Administrator")) return NotFound();
        }

        var postList = forumPosts.ToList();
        var forumPostsCount = postList.Count;
        var totalPages = (int)Math.Ceiling((double)forumPostsCount / PageSize);
        var currentPage = page ?? 1;
        
        forumPosts = postList.Skip((currentPage - 1) * PageSize).Take(PageSize);
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

        if (forumThread == null || forumThread.IsLocked) return BadRequest();
        
        var forumPost = new ForumPost
        {
            ThreadId = forumThread.Id
        };
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
        
        if (forumThread == null || forumThread.IsLocked) return BadRequest();

        // If post has no content, redirect to thread.
        if (forumPost.Content.IsNullOrEmpty())
        {
            return RedirectToAction("ForumPostView", "ForumPost", new { forumThreadId });
        }
        else
        {
            var addPost = new ForumPost
            {
                Content = forumPost.Content,
                ThreadId = forumPost.ThreadId,
                CreatedAt = DateTime.UtcNow,
                CreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id
            };
        
            var result = await _forumPostRepository.CreateNewForumPost(addPost);

            if (result)
            {
                // Get last page
                var forumPosts = await _forumPostRepository.GetAllForumPostsByThreadId(forumThreadId);
                if (forumPosts != null)
                {
                    var forumPostsCount = forumPosts.Count();
                    var totalPages = (int)Math.Ceiling((double)forumPostsCount / PageSize);
        
                    return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId, page = totalPages});
                }
                return NotFound();
            }
        }
        
        return StatusCode(500);
    }

    [Authorize]
    [HttpGet]
    [Route("/ForumPost/Update/{forumPostId}")]
    public async Task<IActionResult> UpdateForumPostContent(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (forumPost == null) return NotFound();
        if (forumPost.Thread.IsLocked) return BadRequest();
        
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.CreatorId
            || HttpContext.User.IsInRole("Moderator") 
            || HttpContext.User.IsInRole("Administrator"))
        {
            return View(forumPost);
        }
        return Forbid();
    }
    
    [Authorize]
    [HttpPost]
    [Route("/ForumPost/Update/{forumPostId}")]
    public async Task<IActionResult> UpdateForumPostContent(int forumPostId, ForumPost forumPost)
    {
        var forumThread = _forumThreadRepository.GetForumThreadById(forumPost.ThreadId).Result;
        if (forumThread is { IsLocked: true }) return BadRequest();
        
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.CreatorId
            || HttpContext.User.IsInRole("Moderator") 
            || HttpContext.User.IsInRole("Administrator"))
        {
            if (forumPost.Content.IsNullOrEmpty())
            {
                ModelState.AddModelError("Content","Cannot have empty post");
                return View(forumPost);
            }
            forumPost.EditedAt = DateTime.Now;
            forumPost.EditedBy = _userManager.GetUserAsync(User).Result.Id;
            
            var result = await _forumPostRepository.UpdateForumPost(forumPost);

            if (result)
            {
                //Needed for RedirectToAction
                var forumThreadId = forumPost.ThreadId;
                return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId});
            }

            return StatusCode(500);
        }
        return Forbid();
    }
    
    [Authorize]
    [HttpGet]
    [Route("/ForumPost/Delete/{forumPostId}")]
    public async Task<IActionResult> DeleteSelectedForumPost(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (forumPost == null) return NotFound();
        
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.CreatorId
            || HttpContext.User.IsInRole("Moderator")
            || HttpContext.User.IsInRole("Administrator"))
        {
            return View(forumPost);
        }
        return Forbid();
    }
    
    
    [Authorize(Roles = "Administrator,Moderator")]
    [HttpPost]
    public async Task<IActionResult> PermaDeleteSelectedForumPostConfirmed(int forumPostId)
    {
        //Needed for RedirectToAction
        var forumPost = _forumPostRepository.GetForumPostById(forumPostId).Result;
        if (forumPost != null)
        {
            var forumThreadId = forumPost.ThreadId;
            
            var result = await _forumPostRepository.DeleteForumPost(forumPostId);

            if (result) return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId});

            return StatusCode(500);
        }

        return BadRequest();
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SoftDeleteSelectedForumPostContent(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (forumPost == null) return BadRequest();
        if (_userManager.GetUserAsync(User).Result.Id == forumPost.CreatorId
            || HttpContext.User.IsInRole("Moderator") 
            || HttpContext.User.IsInRole("Administrator"))
        {
            //Needed for RedirectToAction
            var forumThreadId = forumPost.ThreadId;
            forumPost.IsSoftDeleted = true;
            await UpdateForumPostContent(forumPostId,forumPost);
            return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId});
        }
        return Forbid();
    }
    
    [Authorize(Roles = "Administrator")]
    [HttpGet]
    [Route("/ForumPost/Undelete/{forumPostId}")]
    public async Task<IActionResult> UnDeleteSelectedForumPost(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (forumPost == null) return NotFound();

        return View(forumPost);
    }
    
    [Authorize(Roles="Administrator")]
    [HttpPost]
    public async Task<IActionResult> UnSoftDeleteSelectedForumPostContent(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (forumPost == null) return BadRequest();
        
        forumPost.IsSoftDeleted = false;
        await UpdateForumPostContent(forumPostId,forumPost);
        
        //Needed for RedirectToAction
        var forumThreadId = forumPost.ThreadId;
        return RedirectToAction("ForumPostView", "ForumPost",new {forumThreadId});
    }
}
