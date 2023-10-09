using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum2.DAL;

namespace Forum2.Controllers;

public class ForumCategoryController : Controller
{
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IForumCategoryRepository _forumCategoryRepository;
    
    public ForumCategoryController(IAccountRoleRepository accountRoleRepository, IForumCategoryRepository forumCategoryRepository)
    {
        _accountRoleRepository = accountRoleRepository;
        _forumCategoryRepository = forumCategoryRepository;
    }
    
    public async Task<IActionResult> ForumCategoryTable()
    {
        var forumCategories = await _forumCategoryRepository.GetAll();
        var forumThreads = await _forumThreadRepository.GetAll();
        var forumCategoriesListViewModel = new ForumCategoryViewModel(forumCategories, forumThreads,"ForumCategoryTable");
        return View(forumCategoriesListViewModel);
    }
}