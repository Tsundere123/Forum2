using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum2.Controllers;

public class ForumCategoryController : Controller
{
    private readonly AccountDbContext _accountDbContext;
    private readonly ForumDbContext _forumDbContext;
    
    public ForumCategoryController(AccountDbContext accountDbContext, ForumDbContext forumDbContext)
    {
        _accountDbContext = accountDbContext;
        _forumDbContext = forumDbContext;
    }
    
    public async Task<IActionResult> ForumCategoryTable()
    {
        List<ForumCategory> forumCategories = await _forumDbContext.ForumCategory.ToListAsync();
        List<ForumThread> forumThreads = await _forumDbContext.ForumThread.ToListAsync();
        var forumCategoriesListViewModel = new ForumCategoryViewModel(forumCategories, forumThreads,"ForumCategoryTable");
        return View(forumCategoriesListViewModel);
    }
}