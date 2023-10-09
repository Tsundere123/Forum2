using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum2.DAL;

namespace Forum2.Controllers;

public class ForumThreadController : Controller
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IForumCategoryRepository _forumCategoryRepository;
    private readonly IForumThreadRepository _forumThreadRepository;
    

    public ForumThreadController(IAccountRepository accountRepository, IAccountRoleRepository accountRoleRepository, IForumCategoryRepository forumCategoryRepository, IForumThreadRepository forumThreadRepository)
    {
        _forumCategoryRepository = forumCategoryRepository;
        _forumThreadRepository = forumThreadRepository;
        _accountRoleRepository = accountRoleRepository;
        _accountRepository = accountRepository;
    }
    public async Task<IActionResult> ForumThreadTable()
    {
        var forumThreads = await _forumThreadRepository.GetAll();
        var forumCategories = await _forumCategoryRepository.GetAll();
        var accounts = await _accountRepository.GetAll();
        var forumListViewModel = new ForumListViewModel(forumCategories,forumThreads,accounts);
        return View(forumListViewModel);
    }
}