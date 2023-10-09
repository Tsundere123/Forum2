using Forum2.DAL;
using Microsoft.EntityFrameworkCore;
using Forum2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class AccountRoleController : Controller
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    public async Task<IActionResult> Table()
    {
        var accountRolesList = await _accountRoleRepository.GetAll();
        return View(accountRolesList);
    }
}