using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum2.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


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
    public async Task<IActionResult> ForumThreadTable()
    {
        var forumThreads = await _forumThreadRepository.GetAll();
        var forumCategories = await _forumCategoryRepository.GetAll();
        var accounts = await _userManager.Users.ToListAsync();
        var forumListViewModel = new ForumListViewModel
        {
            ForumCategories = forumCategories,
            ForumThreads = forumThreads,
            Accounts = accounts,
        };
        return View(forumListViewModel);
    }
    [HttpGet]
    [Route("/Category/{forumCategoryId}/{page?}")]
    public async Task<IActionResult> ForumThreadOfCategoryTable(int forumCategoryId, int? page)
    {
        var forumThreads = await _forumThreadRepository.GetForumThreadsByCategoryId(forumCategoryId);
        var forumCategory = await _forumCategoryRepository.GetForumCategoryById(forumCategoryId);
        var accounts = await _userManager.Users.ToListAsync();
        
        // Remove soft deleted threads
        forumThreads = forumThreads.Where(x => x.IsSoftDeleted == false).ToList();
        
        // Sort threads by last post
        var sortedThreads = forumThreads
            .Select(t => new 
            {
                ForumThread = t,
                LastPost = t.Posts.Any() ? t.Posts.Max(p => p.CreatedAt) : t.CreatedAt
            })
            .OrderByDescending(t => t.LastPost)
            .Select(t => t.ForumThread);

        var perPage = 10;
        var totalPages = (int)Math.Ceiling((double)sortedThreads.Count() / perPage);
        var currentPage = page ?? 1;
        var forumThreadsOfCategory = sortedThreads.Skip((currentPage - 1) * perPage).Take(perPage);
        
        var forumThreadOfCategoryViewModel = new ForumThreadOfCategoryViewModel
        {
            ForumCategory = forumCategory,
            ForumThreads = forumThreadsOfCategory,
            Accounts = accounts,
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
        var accounts = await _userManager.Users.ToListAsync();
        var forumThread = new ForumThread();
        var viewModel = new ForumThreadCreationViewModel
        {
            ForumCategory = forumCategory,
            ForumThread = forumThread,
            Accounts = accounts
        };
        return View(viewModel);
    }
    
    [Authorize]
    [HttpPost]
    [Route("/Category/{categoryId}/NewThread")]
    public async Task<IActionResult> CreateNewForumThread(ForumCategory forumCategory,ForumThread forumThread, ForumPost forumPost)
    {
        ForumThread addThread = new ForumThread();
        addThread.Title = forumThread.Title;
        addThread.CreatedAt = DateTime.UtcNow;
        addThread.CreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
        addThread.CategoryId = forumCategory.Id;
        
        await _forumThreadRepository.CreateNewForumThread(addThread);
        int forumThreadId = addThread.Id;
        CreateNewForumPost(forumThreadId, forumPost);
        return RedirectToAction("ForumPostView", "ForumPost", new {forumThreadId});
    }
    [Authorize]
    [HttpPost]
    public async void CreateNewForumPost(int threadId,ForumPost forumPost)
    {
        ForumPost addPost = new ForumPost();
        addPost.CreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
        addPost.CreatedAt = DateTime.UtcNow;
        addPost.ThreadId = threadId;
        addPost.Content = forumPost.Content;
        await _forumPostRepository.CreateNewForumPost(addPost);
    }
    [Authorize]
    [HttpGet]
    [Route("ForumThread/Update/{forumThreadId}")]
    public async Task<IActionResult> UpdateForumThreadTitle(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);

        if (_userManager.GetUserAsync(User).Result.Id == forumThread.CreatorId
            || HttpContext.User.IsInRole("Moderator") 
            || HttpContext.User.IsInRole("Administrator"))
        {
            if (forumThread == null) return NotFound();
            return View(forumThread);
        }
        return Forbid();
    }

    [Authorize]
    [HttpPost]
    [Route("ForumThread/Update/{forumThreadId}")]
    public async Task<IActionResult> UpdateForumThreadTitle(ForumThread forumThread)
    {
        if (_userManager.GetUserAsync(User).Result.Id == forumThread.CreatorId
            || HttpContext.User.IsInRole("Moderator")
            || HttpContext.User.IsInRole("Administrator"))
        {
            if (ModelState.IsValid)
            {
                forumThread.EditedAt = DateTime.Now;
                forumThread.EditedBy = _userManager.GetUserAsync(User).Result.Id;
                await _forumThreadRepository.UpdateForumThread(forumThread);
                //Needed for RedirectToAction
                var forumCategoryId = forumThread.CategoryId;
                return RedirectToAction("ForumThreadOfCategoryTable", "ForumThread", new { forumCategoryId });
            }
            return View(forumThread);
        }
        return Forbid();
    }
    
    [Authorize]
    [HttpGet]
    [Route("ForumThread/Delete/{forumThreadId}")]
    public async Task<IActionResult> DeleteSelectedForumThread(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (_userManager.GetUserAsync(User).Result.Id == forumThread.CreatorId
            || HttpContext.User.IsInRole("Moderator")
            || HttpContext.User.IsInRole("Administrator"))
        {
            if (forumThread == null) return NotFound();
            return View(forumThread);
        }
        return Forbid();
    }
    
    [Authorize(Roles = "Administrator,Moderator")]
    [HttpPost]
    public async Task<IActionResult> PermaDeleteSelectedForumThreadConfirmed(int forumThreadId)
    {
        //Needed for RedirectToAction
        var forumCategoryId = _forumThreadRepository.GetForumThreadById(forumThreadId).Result.CategoryId;
        await _forumThreadRepository.DeleteForumThread(forumThreadId);
        return RedirectToAction("ForumThreadOfCategoryTable", "ForumThread",new { forumCategoryId});
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SoftDeleteSelectedForumThreadConfirmed(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (_userManager.GetUserAsync(User).Result.Id == forumThread.CreatorId
            || HttpContext.User.IsInRole("Moderator")
            || HttpContext.User.IsInRole("Administrator"))
        {
            //Needed for RedirectToAction
            var forumCategoryId = forumThread.CategoryId;
        
            if (forumThread == null) return NotFound();

            forumThread.IsSoftDeleted = true;
            await UpdateForumThreadTitle(forumThread);
            
            //Soft deletes all forumposts as well
            foreach (var forumPost in _forumPostRepository.GetAllForumPostsByThreadId(forumThreadId).Result)
            {
                forumPost.IsSoftDeleted = true;
                await _forumPostRepository.UpdateForumPost(forumPost);
            }
            
            return RedirectToAction("ForumThreadOfCategoryTable", "ForumThread",new { forumCategoryId});

        }
        return Forbid();
    }
    
}