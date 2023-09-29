using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum2.Controllers;

public class AccountController : Controller
{
    private readonly AccountDbContext _accountDbContext;

    public AccountController(AccountDbContext accountDbContext)
    {
        _accountDbContext = accountDbContext;
    }
    
    
    public async Task<IActionResult> Table()
    {
        List<Account> accounts = await _accountDbContext.Accounts.ToListAsync();
        var accountListviewModel = new AccountListViewModel(accounts, "Table");
        return View(accountListviewModel);
    }
    public async Task<IActionResult> Grid()
    {
        List<Account> accounts = await _accountDbContext.Accounts.ToListAsync();
        var accountListviewModel = new AccountListViewModel(accounts, "Grid");
        return View(accountListviewModel);
    }

    public async Task<IActionResult> Details(int accountId)
    {
        var account = await _accountDbContext.Accounts.FirstOrDefaultAsync(i => i.AccountId == accountId);
        if (account == null) return NotFound();
        return View(account);
    }

    [HttpGet]
    public IActionResult CreateNewAccount()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewAccount(Account account)
    {
        if (ModelState.IsValid)
        {
            _accountDbContext.Accounts.Add(account);
            await _accountDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Table));
        }
        return View(account);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateSelectedAccount(int accountId)
    {
        var account = await _accountDbContext.Accounts.FindAsync(accountId);
        if (account == null) return NotFound();
        return View(account);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSelectedAccount(Account account)
    {
        if (ModelState.IsValid)
        {
            _accountDbContext.Accounts.Update(account);
            await _accountDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Table));
        }

        return View(account);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteSelectedAccount(int accountId)
    {
        var account = await _accountDbContext.Accounts.FindAsync(accountId);
        if (account == null) return NotFound();
        return View(account);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDeletedSelectedAccount(int accountId)
    {
        var account = await _accountDbContext.Accounts.FindAsync(accountId);
        if (account == null) return NotFound();
        _accountDbContext.Accounts.Remove(account);
        await _accountDbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Table));
    }
    // public IActionResult Table()
    // {
    //     var accounts = GetAccounts();
    //     ViewBag.CurrentViewName = "Table";
    //     
    //     return View(accounts);
    // }
    //
    // public IActionResult Grid()
    // {
    //     var accounts = GetAccounts();
    //     ViewBag.CurrentViewName = "Grid";
    //     
    //     return View(accounts);
    // }

    public List<Account> GetAccounts()
    {
        var accounts = new List<Account>();
        var account1 = new Account
        {
            AccountId = 1,
            AccountName = "Admin",
            AccountPassword = "yes",
            AccountAvatar = "https://www.ikea.com/no/no/images/products/blahaj-toyleke-hai__0710175_pe727378_s5.jpg?f=s"
        };

        var account2 = new Account
        {
            AccountId = 2,
            AccountName = "Moderator",
            AccountPassword = "no",
            AccountAvatar = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/6e3a4157-8314-48a1-8b90-b03e1643ddee/d8qezl2-3f8544dc-4bb9-4a2f-95e8-77dffd19924a.png/v1/fill/w_750,h_750/kancolle_zuikaku_render_by_ryn_renders_d8qezl2-fullview.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7ImhlaWdodCI6Ijw9NzUwIiwicGF0aCI6IlwvZlwvNmUzYTQxNTctODMxNC00OGExLThiOTAtYjAzZTE2NDNkZGVlXC9kOHFlemwyLTNmODU0NGRjLTRiYjktNGEyZi05NWU4LTc3ZGZmZDE5OTI0YS5wbmciLCJ3aWR0aCI6Ijw9NzUwIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmltYWdlLm9wZXJhdGlvbnMiXX0.B6O1JlYtBqq7K527gy3XW7xPaHYSVQXbqljdJfU-G7I"
        };

        accounts.Add(account1);
        accounts.Add(account2);
        return accounts;
    }
}