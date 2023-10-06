using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum2.Controllers;

public class ForumThreadController : Controller
{
    private readonly ForumDbContext _forumDbContext;
    private readonly AccountDbContext _accountDbContext;

    public ForumThreadController(ForumDbContext forumDbContext, AccountDbContext accountDbContext)
    {
        _forumDbContext = forumDbContext;
        _accountDbContext = accountDbContext;
    }
    public async Task<IActionResult> ForumThreadTable()
    {
        List<ForumThread> forumThreads = await _forumDbContext.ForumThread.ToListAsync();
        List<ForumCategory> forumCategories = await _forumDbContext.ForumCategory.ToListAsync();
        List<Account> accounts = await _accountDbContext.Accounts.ToListAsync();
        var forumListViewModel = new ForumListViewModel(forumCategories,forumThreads,accounts);
        return View(forumListViewModel);
    }
}