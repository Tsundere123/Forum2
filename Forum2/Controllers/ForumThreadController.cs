﻿using Forum2.Models;
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
    [Route("/Category/{forumCategoryId}/{page?}")]
    public async Task<IActionResult> ForumThreadOfCategoryTable(int forumCategoryId, int? page)
    {
        var forumCategory = await _forumCategoryRepository.GetForumCategoryById(forumCategoryId);
        if (forumCategory == null) return NotFound();
        
        var forumThreads = await _forumThreadRepository.GetForumThreadsByCategoryId(forumCategoryId);

        if (!User.IsInRole("Administrator"))
        {
            // Remove soft deleted threads
            forumThreads = forumThreads.Where(x => x.IsSoftDeleted == false);
        }
        
        // Pinned threads
        var pinnedThreads = forumThreads.Where(t => t.IsPinned).ToList();
        
        // Sort threads by last post
        var sortedThreads = forumThreads
            .Where(t => t.IsPinned == false)
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
        var forumThreadsOfCategory = sortedThreads.Skip((currentPage - 1) * perPage).Take(perPage).ToList();
        
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
    public async Task<IActionResult> CreateNewForumThread(ForumThreadCreationViewModel model)
    {
        // Remove validation for fields that are not relevant for this view
        ModelState.Remove("ForumCategory.Description");
        ModelState.Remove("ForumPost.CreatorId");
        
        if (!ModelState.IsValid) return View(model);
        
        ForumThread addThread = new ForumThread();
        addThread.Title = model.ForumThread.Title;
        addThread.CreatedAt = DateTime.UtcNow;
        addThread.CreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
        addThread.CategoryId = model.ForumCategory.Id;
        
        await _forumThreadRepository.CreateNewForumThread(addThread);
        int forumThreadId = addThread.Id;
        CreateNewForumPost(forumThreadId, model.ForumPost);
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
        if (forumThread == null || forumThread.IsLocked) return NotFound();
        
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
        
        if (_userManager.GetUserAsync(User).Result.Id == forumThread.CreatorId
            || HttpContext.User.IsInRole("Moderator")
            || HttpContext.User.IsInRole("Administrator"))
        {
            if (!ModelState.IsValid) return View(forumThread);
            
            forumThread.EditedAt = DateTime.Now;
            forumThread.EditedBy = _userManager.GetUserAsync(User).Result.Id;
            await _forumThreadRepository.UpdateForumThread(forumThread);
            //Needed for RedirectToAction
            var forumCategoryId = forumThread.CategoryId;
            return RedirectToAction("ForumThreadOfCategoryTable", "ForumThread", new { forumCategoryId });
        }
        return Forbid();
    }
    
    [Authorize]
    [HttpGet]
    [Route("ForumThread/Delete/{forumThreadId}")]
    public async Task<IActionResult> DeleteSelectedForumThread(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (forumThread == null) return NotFound();

        
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
        if (forumCategoryId == null) return NotFound();
        
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
    
    [Authorize(Roles = "Administrator")]
    [HttpGet]
    [Route("ForumThread/Undelete/{forumThreadId}")]
    public async Task<IActionResult> UnDeleteSelectedForumThread(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
        if (forumThread == null) return NotFound();


        if (forumThread == null) return NotFound();
        return View(forumThread);

    }
    
    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> UnSoftDeleteSelectedForumThreadConfirmed(int forumThreadId)
    {
        var forumThread = await _forumThreadRepository.GetForumThreadById(forumThreadId);
            //Needed for RedirectToAction
            var forumCategoryId = forumThread.CategoryId;
        
            if (forumThread == null) return NotFound();

            forumThread.IsSoftDeleted = false;
            await UpdateForumThreadTitle(forumThread);
            
            //Unsoft deletes all forumposts as well
            foreach (var forumPost in _forumPostRepository.GetAllForumPostsByThreadId(forumThreadId).Result)
            {
                forumPost.IsSoftDeleted = false;
                await _forumPostRepository.UpdateForumPost(forumPost);
            }
            
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
        
        await _forumThreadRepository.UpdateForumThread(forumThread);
        return RedirectToAction("ForumPostView", "ForumPost", new { forumThreadId });
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

        await _forumThreadRepository.UpdateForumThread(forumThread);
        return RedirectToAction("ForumPostView", "ForumPost", new { forumThreadId });
    }
    
}