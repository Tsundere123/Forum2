using Forum2.DAL;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum2.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    public AccountController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IActionResult> Table()
    {
        var accounts = await _userManager.Users.ToListAsync();
        var accountListviewModel = new AccountListViewModel(accounts, "Table");
        return View(accountListviewModel);
    }
    public async Task<IActionResult> Grid()
    {
        var accounts = await _userManager.Users.ToListAsync();
        var accountListviewModel = new AccountListViewModel(accounts, "Grid");
        return View(accountListviewModel);
    }

    public async Task<IActionResult> Details(string? accountId)
    {
        var account = await _userManager.FindByIdAsync(accountId);
        // var account = await _accountRepository.GetAccountById(accountId);
        if (account == null) return BadRequest("Account not found");
        return View(account);
    }
    
}