using Forum2.DAL;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class SearchController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IForumThreadRepository _threadRepository;
    private readonly IForumPostRepository _postRepository;
    
    public SearchController(UserManager<ApplicationUser> userManager, IForumThreadRepository threadRepository, 
        IForumPostRepository postRepository)
    {
        _userManager = userManager;
        _threadRepository = threadRepository;
        _postRepository = postRepository;
    }
    
    // For consistency, use the same number of items per page for all lists
    private const int CountPerPage = 10;

    [HttpGet]
    public async Task<IActionResult> Index(string? query)
    {
        var model = new SearchResultViewModel();
        if (query == null) return View(model);
        
        var threads = await _threadRepository.GetAll();
        var posts = await _postRepository.GetAll();
        var users = _userManager.Users.ToList();
        
        // Upper case for case-insensitive search
        var threadsToShow = (threads ?? Array.Empty<ForumThread>())
            .Where(t => t.Title.ToUpper().Contains(query.ToUpper())).Take(CountPerPage).ToList();
        
        var postsToShow = (posts ?? Array.Empty<ForumPost>())
            .Where(p => p.Content.ToUpper().Contains(query.ToUpper())).Take(CountPerPage).ToList();
        
        var usersToShow = users
            .Where(u => u.DisplayName.ToUpper().Contains(query.ToUpper())).Take(CountPerPage).ToList();
        
        model.Query = query;
        model.Threads = threadsToShow;
        model.Posts = postsToShow;
        model.Users = usersToShow;

        return View(model);
    }
    
    [HttpGet]
    [Route("/Search/Threads")]
    public async Task<IActionResult> Threads(string? query, int? page)
    {
        var model = new SearchResultViewModel();
        if (query == null) return View(model);
        
        var threads = await _threadRepository.GetAll();
        
        // Upper case for case-insensitive search
        var threadsRelevant = (threads ?? Array.Empty<ForumThread>())
            .Where(t => t.Title.ToUpper().Contains(query.ToUpper())).ToList();
        
        var threadsCount = threadsRelevant.Count;
        var totalPages = (int) Math.Ceiling((double) threadsCount / CountPerPage);
        var currentPage = page ?? 1;
        var threadsToShow = threadsRelevant.Skip((currentPage - 1) * CountPerPage).Take(CountPerPage).ToList();
        
        model.Query = query;
        model.Threads = threadsToShow;
        model.CurrentPage = currentPage;
        model.TotalPages = totalPages;

        return View(model);
    }

    [HttpGet]
    [Route("/Search/Posts")]
    public async Task<IActionResult> Posts(string? query, int? page)
    {
        var model = new SearchResultViewModel();
        if (query == null) return View(model);
        
        var posts = await _postRepository.GetAll();
        
        // Upper case for case-insensitive search
        var postsRelevant = (posts ?? Array.Empty<ForumPost>())
            .Where(p => p.Content.ToUpper().Contains(query.ToUpper())).ToList();

        var postsCount = postsRelevant.Count;
        var totalPages = (int) Math.Ceiling((double) postsCount / CountPerPage);
        var currentPage = page ?? 1;
        var postsToShow = postsRelevant.Skip((currentPage - 1) * CountPerPage).Take(CountPerPage).ToList();
        
        model.Query = query;
        model.Posts = postsToShow;
        model.CurrentPage = currentPage;
        model.TotalPages = totalPages;

        return View(model);
    }
    
    [HttpGet]
    [Route("/Search/Users")]
    public IActionResult Users(string? query, int? page)
    {
        var model = new SearchResultViewModel();
        if (query == null) return View(model);
        
        var users = _userManager.Users.ToList();
        
        // Upper case for case-insensitive search
        var usersRelevant = users.Where(u => u.DisplayName.ToUpper().Contains(query.ToUpper())).ToList();
        
        var userCount = usersRelevant.Count;
        var totalPages = (int) Math.Ceiling((double) userCount / CountPerPage);
        var currentPage = page ?? 1;
        var usersToShow = usersRelevant.Skip((currentPage - 1) * CountPerPage).Take(CountPerPage).ToList();
        
        model.Query = query;
        model.Users = usersToShow;
        model.CurrentPage = currentPage;
        model.TotalPages = totalPages;

        return View(model);
    }
}