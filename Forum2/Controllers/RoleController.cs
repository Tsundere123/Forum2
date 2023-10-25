using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum2.Controllers;

public class RoleController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleController(RoleManager<ApplicationRole> roleManager,UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> RoleTable()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }
    
    public async Task<IActionResult> MembersOfRole(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        var users = await _userManager.Users.ToListAsync();
        var RoleViewModel = new RoleViewModel(users, role);
        return View(RoleViewModel);
    }
}