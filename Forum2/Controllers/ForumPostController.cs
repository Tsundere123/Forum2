using Forum2.DAL;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class ForumPostController : Controller
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IForumCategoryRepository _forumCategoryRepository;
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ForumPostController(IAccountRepository accountRepository, IAccountRoleRepository accountRoleRepository,
        IForumCategoryRepository forumCategoryRepository, IForumThreadRepository forumThreadRepository,IForumPostRepository forumPostRepository, UserManager<ApplicationUser> userManager)
    {
        _forumCategoryRepository = forumCategoryRepository;
        _forumThreadRepository = forumThreadRepository;
        _accountRoleRepository = accountRoleRepository;
        _accountRepository = accountRepository;
        _forumPostRepository = forumPostRepository;
        _userManager = userManager;
    }
    [HttpGet]
    public async Task<IActionResult> ForumPostView(int threadId)
    {
        var forumPosts = await _forumPostRepository.GetAllForumPostsByThreadId(threadId);
        var forumCategory = await _forumCategoryRepository.GetForumCategoryById(_forumThreadRepository.GetForumThreadById(threadId).Result.ForumCategoryId);
        var currentForumThread = await _forumThreadRepository.GetForumThreadById(threadId);
        var accounts = await _accountRepository.GetAll();
        var forumPostViewModel = new ForumPostViewModel(forumCategory, currentForumThread, forumPosts, accounts);
        
        return View(forumPostViewModel);
    }
    [HttpGet]
    public async Task<IActionResult> CreateNewForumPost(int threadId)
    {
        // var thread = _forumThreadRepository.GetForumThreadById(threadId);
        // var posts = _forumPostRepository.
        // var viewModel = new ForumPostCreationViewModel(thread,);
        // viewModel.ForumThread.ForumThreadId = threadId;

        var forumThread = await _forumThreadRepository.GetForumThreadById(threadId);
        var accounts = await _accountRepository.GetAll();
        var forumPost = new ForumPost();
        forumPost.ForumThreadId = forumThread.ForumThreadId;
        var viewModel = new ForumPostCreationViewModel(forumThread, forumPost, accounts);
        return View(viewModel);
    }
    public async Task<IActionResult> CreateNewForumPost(ForumPost forumPost)
    {
        ForumPost addPost = new ForumPost();
        addPost.ForumPostContent = forumPost.ForumPostContent;
        // addPost.ForumThreadId = forumPost.ForumThreadId;
        addPost.ForumThreadId = forumPost.ForumThreadId;
        addPost.ForumPostCreationTimeUnix = DateTime.UtcNow;
        addPost.ForumPostCreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
        await _forumPostRepository.CreateNewForumPost(addPost);

        var threadId = forumPost.ForumThreadId;
        return RedirectToAction("ForumPostView", "ForumPost",new {threadId});
        // return View(nameof(ForumPostView),viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateForumPostContent(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (forumPost == null) return NotFound();
        return View(forumPost);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateForumPostContent(ForumPost forumPost)
    {
        if (ModelState.IsValid)
        {
            await _forumPostRepository.UpdateForumPost(forumPost);
            var threadId = forumPost.ForumThreadId;
            return RedirectToAction("ForumPostView", "ForumPost",new {threadId});
        }
        return View(forumPost);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteSelectedForumPost(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        if (forumPost == null) return NotFound();
        return View(forumPost);
    }
    

    [HttpPost]
    public async Task<IActionResult> PermaDeleteSelectedForumPostConfirmed(int forumPostId)
    {
        var threadId = _forumPostRepository.GetForumPostById(forumPostId).Result.ForumThreadId;
        await _forumPostRepository.DeleteForumPost(forumPostId);
        return RedirectToAction("ForumPostView", "ForumPost",new {threadId});
    }
    
    [HttpPost]
    public async Task<IActionResult> SoftDeleteSelectedForumPostContent(int forumPostId)
    {
        var forumPost = await _forumPostRepository.GetForumPostById(forumPostId);
        var threadId = forumPost.ForumThreadId;
        if (forumPost == null) return NotFound();
        forumPost.ForumPostContent = "This post has been deleted";
        await UpdateForumPostContent(forumPost);
        return RedirectToAction("ForumPostView", "ForumPost",new {threadId});
    }
}
