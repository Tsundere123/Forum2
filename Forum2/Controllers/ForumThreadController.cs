using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Forum2.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Forum2.Controllers;

public class ForumThreadController : Controller
{
    private readonly IForumCategoryRepository _forumCategoryRepository;
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ForumThreadController(IForumCategoryRepository forumCategoryRepository, IForumThreadRepository forumThreadRepository,
        IForumPostRepository forumPostRepository, UserManager<ApplicationUser> userManager)
    {
        _forumCategoryRepository = forumCategoryRepository;
        _forumThreadRepository = forumThreadRepository;
        _userManager = userManager;
        _forumPostRepository = forumPostRepository;
    }

    [HttpGet]
    [Route("/Category/{forumCategoryId}/{page?}")]
    public async Task<IActionResult> ForumThreadOfCategoryTable(int forumCategoryId, int? page)
    {
        var forumCategory = await _forumCategoryRepository.GetForumCategoryById(forumCategoryId);
        if (forumCategory == null) return NotFound();
        
        var forumThreads = await _forumThreadRepository.GetForumThreadsByCategoryId(forumCategoryId);
        if (forumThreads == null) return NotFound();

        if (!User.IsInRole("Administrator"))
        {
            // Remove soft deleted threads
            forumThreads = forumThreads.Where(x => x.IsSoftDeleted == false);
        }
        
        // Pinned threads
        var threadList = forumThreads.ToList();
        var pinnedThreads = threadList.Where(t => t.IsPinned).ToList();
        
        // Sort threads by last post
        var sortedThreads = threadList
            .Where(t => t.IsPinned == false)
            .Select(t => new
            {
                ForumThread = t,
                LastPost = t.Posts!.Any() ? t.Posts!.Max(p => p.CreatedAt) : t.CreatedAt
            })
            .OrderByDescending(t => t.LastPost)
            .Select(t => t.ForumThread);

        //Pagination
        const int perPage = 10;
        var pageList = sortedThreads.ToList();
        var totalPages = (int)Math.Ceiling((double)pageList.Count / perPage);
        var currentPage = page ?? 1;
        var forumThreadsOfCategory = pageList.Skip((currentPage - 1) * perPage).Take(perPage).ToList();
        
        var forumThreadOfCategoryViewModel = new ForumThreadOfCategoryViewModel
        {
            ForumCategory = forumCategory,
            PinnedThreads = pinnedThreads,
            ForumThreads = forumThreadsOfCategory,
            CurrentPage = currentPage,
            TotalPages = totalPages
        };
        return View(forumThreadOfCategoryViewModel);
    }

    [Authorize]
    [HttpGet]
    [Route("/Category/{categoryId}/NewThread")]
    public async Task<IActionResult> CreateNewForumThread(int categoryId)
    {
        var forumCategory = await _forumCategoryRepository.GetForumCategoryById(categoryId);
        if (forumCategory == null) return NotFound();
        
        var forumThread = new ForumThread();
        var viewModel = new ForumThreadCreationViewModel
        {
            ForumCategory = forumCategory,
            ForumThread = forumThread,
        };
        return View(viewModel);
    }
    
    [Authorize]
    [HttpPost]
    [Route("/Category/{categoryId}/NewThread")]
    public async Task<IActionResult> CreateNewForumThread(int categoryId,ForumThreadCreationViewModel model)
    {
        if (model.ForumThread.Title.IsNullOrEmpty())
        {
            ModelState.AddModelError("ForumThread.Title","Cannot have empty title");
            return View(model);
        }
        if (model.ForumPost.Content.IsNullOrEmpty())
        {
            ModelState.AddModelError("ForumPost.Content","Cannot have empty post");
            return View(model);
        }
        
        var addThread = new ForumThread
        {
            Title = model.ForumThread.Title,
            CreatedAt = DateTime.UtcNow,
            CreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id,
            CategoryId = model.ForumCategory.Id
        };

        var result = await _forumThreadRepository.CreateNewForumThread(addThread);

        if (result)
        {
            var forumThreadId = addThread.Id;
            CreateNewForumPost(forumThreadId, model.ForumPost);
            return RedirectToAction("ForumPostView", "ForumPost", new {forumThreadId});
        }

        return StatusCode(500);
    }
    
    [Authorize]
    [HttpPost]
    public async void CreateNewForumPost(int threadId,ForumPost forumPost)
    {
        var addPost = new ForumPost
        {
            CreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id,
            CreatedAt = DateTime.UtcNow,
            ThreadId = threadId,
            Content = forumPost.Content
        };
        await _forumPostRepository.CreateNewForumPost(addPost);
    }
    
    [Authorize]
    [HttpGet]
    [Route("ForumThread/Update/{forumThreadId}")]
    public async Task<IActionResult> UpdateForumThreadTitle(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (forumThread == null || forumThread.IsLocked) return NotFound();
        
        //Authenticate
        if (_userManager.GetUserAsync(User).Result.Id == forumThread.CreatorId
            || HttpContext.User.IsInRole("Moderator") 
            || HttpContext.User.IsInRole("Administrator"))
        {
            return View(forumThread);
        }
        return Forbid();
    }

    [Authorize]
    [HttpPost]
    [Route("ForumThread/Update/{forumThreadId}")]
    public async Task<IActionResult> UpdateForumThreadTitle(ForumThread forumThread)
    {
        var forumThreadExists = await _forumThreadRepository.GetForumThreadById(forumThread.Id);
        if (forumThreadExists == null || forumThreadExists.IsLocked) return NotFound();

        //Authenticate
        if (_userManager.GetUserAsync(User).Result.Id != forumThread.CreatorId
            && !HttpContext.User.IsInRole("Moderator")
            && !HttpContext.User.IsInRole("Administrator")) return Forbid();
        
        if (forumThread.Title.IsNullOrEmpty())
        {
            ModelState.AddModelError("Title","Title cannot be empty");
            return View(forumThread);
        }

        forumThreadExists.Title = forumThread.Title;
        forumThreadExists.EditedAt = DateTime.Now;
        forumThreadExists.EditedBy = _userManager.GetUserAsync(User).Result.Id;
        
        var result = await _forumThreadRepository.UpdateForumThread(forumThreadExists);
        if (result)
        {
            //Needed for RedirectToAction
            var forumCategoryId = forumThread.CategoryId;
            return RedirectToAction("ForumThreadOfCategoryTable", "ForumThread", new { forumCategoryId });
        }

        return StatusCode(500);
    }
    
    [Authorize]
    [HttpGet]
    [Route("ForumThread/Delete/{forumThreadId}")]
    public async Task<IActionResult> DeleteSelectedForumThread(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (forumThread == null) return NotFound();
        
        //Authenticate
        if (_userManager.GetUserAsync(User).Result.Id == forumThread.CreatorId
            || HttpContext.User.IsInRole("Moderator")
            || HttpContext.User.IsInRole("Administrator"))
        {
            return View(forumThread);
        }
        return Forbid();
    }
    
    [Authorize(Roles = "Administrator,Moderator")]
    [HttpPost]
    public async Task<IActionResult> PermaDeleteSelectedForumThreadConfirmed(int forumThreadId)
    {
        //Needed for RedirectToAction
        var forumThread = _forumThreadRepository.GetForumThreadById(forumThreadId).Result;
        if (forumThread == null) return BadRequest();
        
        var forumCategoryId = forumThread.CategoryId;
        
        var result = await _forumThreadRepository.DeleteForumThread(forumThreadId);
        if (result) return RedirectToAction("ForumThreadOfCategoryTable", "ForumThread",new { forumCategoryId});
        return StatusCode(500);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SoftDeleteSelectedForumThreadConfirmed(int forumThreadId)
    {
        var forumPosts = _forumPostRepository.GetAllForumPostsByThreadId(forumThreadId).Result;
        if (forumPosts == null) return NotFound();
        
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        
        //Authenticate
        if (_userManager.GetUserAsync(User).Result.Id != forumThread?.CreatorId
            && !HttpContext.User.IsInRole("Moderator")
            && !HttpContext.User.IsInRole("Administrator")) return Forbid();
        
        if (forumThread == null) return NotFound();
        
        forumThread.IsSoftDeleted = true;
        var result = await _forumThreadRepository.UpdateForumThread(forumThread);
        if (!result) return StatusCode(500);
            
        //Soft deletes all forum posts as well
        foreach (var forumPost in forumPosts)
        {
            forumPost.IsSoftDeleted = true;
            
            // Not checking if soft deleting post is successful. Just continue to next post.
            await _forumPostRepository.UpdateForumPost(forumPost);
        }
        //Needed for RedirectToAction
        var forumCategoryId = forumThread.CategoryId;
        return RedirectToAction("ForumThreadOfCategoryTable", "ForumThread",new { forumCategoryId});
    }
    
    [Authorize(Roles = "Administrator")]
    [HttpGet]
    [Route("ForumThread/Undelete/{forumThreadId}")]
    public async Task<IActionResult> UnDeleteSelectedForumThread(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (forumThread == null) return NotFound();

        return View(forumThread);
    }
    
    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> UnSoftDeleteSelectedForumThreadConfirmed(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (forumThread == null) return NotFound();
        
        forumThread.IsSoftDeleted = false;
        var result = await _forumThreadRepository.UpdateForumThread(forumThread);
        if (!result) return StatusCode(500);
        
        //Un soft deletes all forum posts as well
        var forumPosts = _forumPostRepository.GetAllForumPostsByThreadId(forumThreadId).Result;
        if (forumPosts != null)
        {
            foreach (var forumPost in forumPosts)
            {
                forumPost.IsSoftDeleted = false;
                var resultPost = await _forumPostRepository.UpdateForumPost(forumPost);
                if (!resultPost) return StatusCode(500);
            }
        }
        
        //Needed for RedirectToAction
        var forumCategoryId = forumThread.CategoryId;
        return RedirectToAction("ForumThreadOfCategoryTable", "ForumThread",new { forumCategoryId});
    }
    
    //
    // Pin Thread Toggle
    //
    [HttpPost]
    [Authorize(Roles = "Administrator,Moderator")]
    [Route("/ForumThread/Pin/{forumThreadId}")]
    public async Task<IActionResult> TogglePinSelectedForumThread(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (forumThread == null) return NotFound();
        
        forumThread.IsPinned = !forumThread.IsPinned;
        
        var result = await _forumThreadRepository.UpdateForumThread(forumThread);
        if (result) return RedirectToAction("ForumPostView", "ForumPost", new { forumThreadId });
        return StatusCode(500);
    }
    
    //
    // Lock Thread Toggle
    //
    [HttpPost]
    [Authorize(Roles = "Administrator,Moderator")]
    [Route("/ForumThread/Lock/{forumThreadId}")]
    public async Task<IActionResult> ToggleLockSelectedForumThread(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (forumThread == null) return NotFound();
        
        forumThread.IsLocked = !forumThread.IsLocked;

        var result = await _forumThreadRepository.UpdateForumThread(forumThread);
        if (result) return RedirectToAction("ForumPostView", "ForumPost", new { forumThreadId });
        return StatusCode(500);
    }
    
}