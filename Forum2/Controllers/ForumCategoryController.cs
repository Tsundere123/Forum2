using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum2.DAL;

namespace Forum2.Controllers;

public class ForumCategoryController : Controller
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IForumCategoryRepository _forumCategoryRepository;
    private readonly IForumThreadRepository _forumThreadRepository;
    
    public ForumCategoryController(IAccountRepository accountRepository, IAccountRoleRepository accountRoleRepository, IForumCategoryRepository forumCategoryRepository, IForumThreadRepository forumThreadRepository)
    {
        _forumCategoryRepository = forumCategoryRepository;
        _forumThreadRepository = forumThreadRepository;
        _accountRoleRepository = accountRoleRepository;
        _accountRepository = accountRepository;
    }
    [Route("/Category")]
    public async Task<IActionResult> ForumCategoryTable()
    {
        var forumCategories = await _forumCategoryRepository.GetAll();
        var forumThreads = await _forumThreadRepository.GetAll();
        var forumCategoriesListViewModel = new ForumCategoryViewModel(forumCategories, forumThreads,"ForumCategoryTable");
        return View(forumCategoriesListViewModel);
    }
}