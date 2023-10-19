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
    
    public SearchController(UserManager<ApplicationUser> userManager, IForumThreadRepository threadRepository, IForumPostRepository postRepository)
    {
        _userManager = userManager;
        _threadRepository = threadRepository;
        _postRepository = postRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? query)
    {
        var viewModel = new SearchResultViewModel();
        if (query == null) return View(viewModel);
        
        var threads = await _threadRepository.GetAll();
        var posts = await _postRepository.GetAll();
        var users = _userManager.Users.ToList();
        
        viewModel.Query = query;

        var limit = 10;
        viewModel.Threads = threads.Where(t => t.ForumThreadTitle.ToUpper().Contains(query.ToUpper())).Take(limit).ToList();
        viewModel.Posts = posts.Where(p => p.ForumPostContent.ToUpper().Contains(query.ToUpper())).Take(limit).ToList();
        viewModel.Users = users.Where(u => u.DisplayName.ToUpper().Contains(query.ToUpper())).Take(limit).ToList();

        return View(viewModel);
    }
    
    [HttpGet]
    [Route("/Search/Threads")]
    public async Task<IActionResult> Threads(string? query, int? page)
    {
        var viewModel = new SearchResultViewModel();
        if (query == null) return View(viewModel);
        
        var threads = await _threadRepository.GetAll();
        var threadsRelevant = threads.Where(t => t.ForumThreadTitle.ToUpper().Contains(query.ToUpper())).ToList();
        
        var threadsCount = threadsRelevant.Count;
        var threadsPerPage = 10;
        var totalPages = (int) Math.Ceiling((double) threadsCount / threadsPerPage);
        var currentPage = page ?? 1;
        var threadsToShow = threadsRelevant.Skip((currentPage - 1) * threadsPerPage).Take(threadsPerPage).ToList();
        
        
        viewModel.Query = query;
        viewModel.Threads = threadsToShow;
        viewModel.CurrentPage = currentPage;
        viewModel.TotalPages = totalPages;

        return View(viewModel);
    }
    
    [HttpGet]
    [Route("/Search/Posts")]
    public async Task<IActionResult> Posts(string? query, int? page)
    {
        var viewModel = new SearchResultViewModel();
        if (query == null) return View(viewModel);
        
        var posts = await _postRepository.GetAll();
        var postsRelevant = posts.Where(p => p.ForumPostContent.ToUpper().Contains(query.ToUpper())).ToList();

        var postsCount = postsRelevant.Count;
        var postsPerPage = 10;
        var totalPages = (int) Math.Ceiling((double) postsCount / postsPerPage);
        var currentPage = page ?? 1;
        var postsToShow = postsRelevant.Skip((currentPage - 1) * postsPerPage).Take(postsPerPage).ToList();
        
        viewModel.Query = query;
        viewModel.Posts = postsToShow;
        viewModel.CurrentPage = currentPage;
        viewModel.TotalPages = totalPages;

        return View(viewModel);
    }
    
    [HttpGet]
    [Route("/Search/Users")]
    public IActionResult Users(string? query, int? page)
    {
        var viewModel = new SearchResultViewModel();
        if (query == null) return View(viewModel);
        
        var users = _userManager.Users.ToList();
        var usersRelevant = users.Where(u => u.DisplayName.ToUpper().Contains(query.ToUpper())).ToList();
        
        var userCount = usersRelevant.Count;
        var usersPerPage = 10;
        var totalPages = (int) Math.Ceiling((double) userCount / usersPerPage);
        var currentPage = page ?? 1;
        var usersToShow = usersRelevant.Skip((currentPage - 1) * usersPerPage).Take(usersPerPage).ToList();
        
        viewModel.Query = query;
        viewModel.Users = usersToShow;
        viewModel.CurrentPage = currentPage;
        viewModel.TotalPages = totalPages;

        return View(viewModel);
    }
}