using Microsoft.EntityFrameworkCore;
using Forum2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class AccountRoleController : Controller
{
    private readonly AccountDbContext _accountDbContext;

    public AccountRoleController(AccountDbContext accountDbContext)
    {
        _accountDbContext = accountDbContext;
    }

    public async Task<IActionResult> Table()
    {
        List<AccountRoles> accountRolesList = await _accountDbContext.AccountRoles.ToListAsync();
        return View(accountRolesList);
    }
}