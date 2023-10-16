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
    }
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
    public IActionResult CreateNewForumPost(int threadId)
    {
        // var thread = _forumThreadRepository.GetForumThreadById(threadId);
        // var posts = _forumPostRepository.
        // var viewModel = new ForumPostCreationViewModel(thread,);
        // viewModel.ForumThread.ForumThreadId = threadId;
        return View();
    }
    public async Task<IActionResult> CreateNewForumPost(int threadId,ForumPost forumPost)
    {
        ForumPost addPost = new ForumPost();
        addPost.ForumPostContent = forumPost.ForumPostContent;
        // addPost.ForumThreadId = forumPost.ForumThreadId;
        addPost.ForumThreadId = threadId;
        addPost.ForumPostCreationTimeUnix = DateTime.UtcNow;
        addPost.ForumPostCreatorId = _userManager.GetUserAsync(HttpContext.User).Result.Id;
        await _forumPostRepository.CreateNewForumPost(addPost);
        return View(nameof(ForumPostView));
    }
}