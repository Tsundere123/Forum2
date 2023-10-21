using System.Net;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum2.DAL;
using Microsoft.AspNetCore.Identity;
using Forum2.Controllers;


namespace Forum2.Controllers;

public class ForumThreadController : Controller
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IForumCategoryRepository _forumCategoryRepository;
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ForumThreadController(IAccountRepository accountRepository, IAccountRoleRepository accountRoleRepository, 
        IForumCategoryRepository forumCategoryRepository, IForumThreadRepository forumThreadRepository,
        IForumPostRepository forumPostRepository, UserManager<ApplicationUser> userManager)
    {
        _forumCategoryRepository = forumCategoryRepository;
        _forumThreadRepository = forumThreadRepository;
        _accountRoleRepository = accountRoleRepository;
        _accountRepository = accountRepository;
        _userManager = userManager;
        _forumPostRepository = forumPostRepository;
    }
    public async Task<IActionResult> ForumThreadTable()
    {
        var forumThreads = await _forumThreadRepository.GetAll();
        var forumCategories = await _forumCategoryRepository.GetAll();
        var accounts = await _accountRepository.GetAll();
        var forumListViewModel = new ForumListViewModel(forumCategories,forumThreads,accounts);
        return View(forumListViewModel);
    }
    public async Task<IActionResult> ForumThreadOfCategoryTable(int id)
    {
        var forumThreads = await _forumThreadRepository.GetForumThreadsByCategoryId(id);
        var forumCategory = await _forumCategoryRepository.GetForumCategoryById(id);
        var accounts = await _accountRepository.GetAll();
        var forumThreadOfCategoryViewModel = new ForumThreadOfCategoryViewModel(forumCategory,forumThreads,accounts);
        return View(forumThreadOfCategoryViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> CreateNewForumThread(int categoryId)
    {
        var forumCategory = await _forumCategoryRepository.GetForumCategoryById(categoryId);
        var accounts = await _accountRepository.GetAll();
        var forumThread = new ForumThread();
        var viewModel = new ForumThreadCreationViewModel(forumCategory, forumThread, null, accounts);
        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> CreateNewForumThread(ForumCategory forumCategory,ForumThread forumThread, ForumPost forumPost)
    {
        ForumThread addThread = new ForumThread();
        addThread.ForumThreadTitle = forumThread.ForumThreadTitle;
        addThread.ForumThreadCreationTimeUnix = DateTime.UtcNow;
        addThread.ForumThreadCreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
        addThread.ForumCategoryId = forumCategory.ForumCategoryId;
        
        await _forumThreadRepository.CreateNewForumThread(addThread);
        int threadId = addThread.ForumThreadId;
        
        CreateNewForumPost(threadId, forumPost);
        return RedirectToAction("ForumPostView", "ForumPost", new {threadId });
    }

    public async void CreateNewForumPost(int threadId,ForumPost forumPost)
    {
        ForumPost addPost = new ForumPost();
        addPost.ForumPostCreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
        addPost.ForumPostCreationTimeUnix = DateTime.UtcNow;
        addPost.ForumThreadId = threadId;
        addPost.ForumPostContent = forumPost.ForumPostContent;
        await _forumPostRepository.CreateNewForumPost(addPost);
    }


    
}