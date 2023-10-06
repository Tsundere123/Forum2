using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum2.Controllers;

public class ForumThreadController : Controller
{
    private readonly ForumDbContext _forumDbContext;

    public ForumThreadController(ForumDbContext forumDbContext)
    {
        _forumDbContext = forumDbContext;
    }
    public async Task<IActionResult> ForumThreadTable()
    {
        List<ForumThread> forumThreads = await _forumDbContext.ForumThread.ToListAsync();
        List<ForumCategory> forumCategories = await _forumDbContext.ForumCategory.ToListAsync();
        var forumListViewModel = new ForumListViewModel(forumCategories,forumThreads);
        return View(forumListViewModel);
    }
}