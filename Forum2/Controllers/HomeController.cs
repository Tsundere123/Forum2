using System.Diagnostics;
using Forum2.DAL;
using Microsoft.AspNetCore.Mvc;
using Forum2.Models;
using Forum2.ViewModels;

namespace Forum2.Controllers;

public class HomeController : Controller
{
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;

    public HomeController(IForumThreadRepository forumThreadRepository, IForumPostRepository forumPostRepository)
    {
        _forumThreadRepository = forumThreadRepository;
        _forumPostRepository = forumPostRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var threads = await _forumThreadRepository.GetAll();
        var posts = await _forumPostRepository.GetAll();
        
        var latestThreads = (threads ?? Array.Empty<ForumThread>())
            .Where(t => t.IsSoftDeleted == false)
            .OrderByDescending(t => t.CreatedAt).Take(5);
        
        var latestPosts = (posts ?? Array.Empty<ForumPost>())
            .Where(p => p.IsSoftDeleted == false)
            .OrderByDescending(p => p.CreatedAt).Take(5);

        var model = new HomeViewModel
        {
            ForumThreads = latestThreads.ToList(),
            ForumPosts = latestPosts.ToList()
        };
        
        return View(model);
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}