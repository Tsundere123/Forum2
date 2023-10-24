using Forum2.DAL;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;

    public ProfileController(UserManager<ApplicationUser> userManager,
        IForumThreadRepository forumThreadRepository, IForumPostRepository forumPostRepository)
    {
        _userManager = userManager;
        _forumThreadRepository = forumThreadRepository;
        _forumPostRepository = forumPostRepository;
    }

    [HttpGet]
    [Route("Profile/{displayName}")]
    public Task<IActionResult> Index(string displayName)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (user == null) return Task.FromResult<IActionResult>(NotFound());

        var model = new ProfileIndexViewModel()
        {
            User = user
        };
        return Task.FromResult<IActionResult>(View(model));
    }

    [HttpGet]
    [Route("/Profile/{displayName}/Threads/{page?}")]
    public async Task<IActionResult> Threads(string displayName, int? page)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (user == null) return NotFound();
        
        var threads = await _forumThreadRepository.GetForumThreadsByAccountId(user.Id);
        var threadsCount = threads.Count();
        var threadsPerPage = 10;
        var totalPages = (int) Math.Ceiling((double) threadsCount / threadsPerPage);
        var currentPage = page ?? 1;
        var threadsToShow = threads.Skip((currentPage - 1) * threadsPerPage).Take(threadsPerPage).ToList();
        
        var model = new ProfileThreadsViewModel
        {
            User = user,
            CurrentPage = currentPage,
            TotalPages = totalPages,
            Threads = threadsToShow
        };
        return View(model);
    }
    
    [HttpGet()]
    [Route("/Profile/{displayName}/Posts/{page?}")]
    public async Task<IActionResult> Posts(string displayName, int? page)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (user == null) return NotFound();
        
        var posts = await _forumPostRepository.GetAllForumPostsByAccountId(user.Id);
        var postsCount = posts.Count();
        var postsPerPage = 10;
        var totalPages = (int) Math.Ceiling((double) postsCount / postsPerPage);
        var currentPage = page ?? 1;
        var postsToShow = posts.Skip((currentPage - 1) * postsPerPage).Take(postsPerPage).ToList();
        
        var model = new ProfilePostsViewModel
        {
            User = user,
            CurrentPage = currentPage,
            TotalPages = totalPages,
            Posts = postsToShow
        };
        return View(model);
    }
}