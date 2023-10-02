using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum2.Controllers;

public class ForumThreadController : Controller
{
    private readonly AccountDbContext _accountDbContext;

    public ForumThreadController(AccountDbContext accountDbContext)
    {
        _accountDbContext = accountDbContext;
    }
    public async Task<IActionResult> ForumThreadTable()
    {
        List<Account> accounts = await _accountDbContext.Accounts.ToListAsync();
        var forumListViewModel = new AccountListViewModel(accounts, "Table");
        return View(forumListViewModel);
    }
}