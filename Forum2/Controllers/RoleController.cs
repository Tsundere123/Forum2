using Forum2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class RoleController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleController(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IActionResult List()
    {
        return View();
    }
    
    [Route("Role/Members/{roleName}")]
    public IActionResult Members(string roleName)
    {
        var role = _roleManager.FindByNameAsync(roleName).Result;
        return View(role);
    }
}