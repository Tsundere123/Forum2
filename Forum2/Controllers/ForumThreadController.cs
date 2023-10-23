using System.Net;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum2.DAL;
using Microsoft.AspNetCore.Identity;
using Forum2.Controllers;
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
        var forumListViewModel = new ForumListViewModel(forumCategories,forumThreads,accounts);
        return View(forumListViewModel);
    }
    [HttpGet]
    [Route("/Category/{forumCategoryId}")]
    public async Task<IActionResult> ForumThreadOfCategoryTable(int forumCategoryId)
    {
        var forumThreads = await _forumThreadRepository.GetForumThreadsByCategoryId(forumCategoryId);
        var forumCategory = await _forumCategoryRepository.GetForumCategoryById(forumCategoryId);
        var accounts = await _userManager.Users.ToListAsync();
        var forumThreadOfCategoryViewModel = new ForumThreadOfCategoryViewModel(forumCategory,forumThreads,accounts);
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
        var viewModel = new ForumThreadCreationViewModel(forumCategory, forumThread, null, accounts);
        return View(viewModel);
    }
    
    [Authorize]
    [HttpPost]
    [Route("/Category/{categoryId}/NewThread")]
    public async Task<IActionResult> CreateNewForumThread(ForumCategory forumCategory,ForumThread forumThread, ForumPost forumPost)
    {
        ForumThread addThread = new ForumThread();
        addThread.ForumThreadTitle = forumThread.ForumThreadTitle;
        addThread.ForumThreadCreationTimeUnix = DateTime.UtcNow;
        addThread.ForumThreadCreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
        addThread.ForumCategoryId = forumCategory.ForumCategoryId;
        
        await _forumThreadRepository.CreateNewForumThread(addThread);
        int forumThreadId = addThread.ForumThreadId;
        CreateNewForumPost(forumThreadId, forumPost);
        return RedirectToAction("ForumPostView", "ForumPost", new {forumThreadId});
    }
    [Authorize]
    [HttpPost]
    public async void CreateNewForumPost(int threadId,ForumPost forumPost)
    {
        ForumPost addPost = new ForumPost();
        addPost.ForumPostCreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
        addPost.ForumPostCreationTimeUnix = DateTime.UtcNow;
        addPost.ForumThreadId = threadId;
        addPost.ForumPostContent = forumPost.ForumPostContent;
        await _forumPostRepository.CreateNewForumPost(addPost);
    }
    [Authorize]
    [HttpGet]
    [Route("ForumThread/Update/{forumThreadId}")]
    public async Task<IActionResult> UpdateForumThreadTitle(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);

        if (_userManager.GetUserAsync(User).Result.Id == forumThread.ForumThreadCreatorId
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
        if (_userManager.GetUserAsync(User).Result.Id == forumThread.ForumThreadCreatorId
            || HttpContext.User.IsInRole("Moderator")
            || HttpContext.User.IsInRole("Administrator"))
        {
            if (ModelState.IsValid)
            {
                await _forumThreadRepository.UpdateForumThread(forumThread);
                //Needed for RedirectToAction
                var forumCategoryId = forumThread.ForumCategoryId;
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
        if (_userManager.GetUserAsync(User).Result.Id == forumThread.ForumThreadCreatorId
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
        var forumCategoryId = _forumThreadRepository.GetForumThreadById(forumThreadId).Result.ForumCategoryId;
        await _forumThreadRepository.DeleteForumThread(forumThreadId);
        return RedirectToAction("ForumThreadOfCategoryTable", "ForumThread",new { forumCategoryId});
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SoftDeleteSelectedForumThreadConfirmed(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (_userManager.GetUserAsync(User).Result.Id == forumThread.ForumThreadCreatorId
            || HttpContext.User.IsInRole("Moderator")
            || HttpContext.User.IsInRole("Administrator"))
        {
            //Needed for RedirectToAction
            var forumCategoryId = forumThread.ForumCategoryId;
        
            if (forumThread == null) return NotFound();

            forumThread.ForumThreadIsSoftDeleted = true;
            await UpdateForumThreadTitle(forumThread);
            return RedirectToAction("ForumThreadOfCategoryTable", "ForumThread",new { forumCategoryId});

        }
        return Forbid();
    }
    
}