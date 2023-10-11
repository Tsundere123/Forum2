using Forum2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

[Authorize( Roles = "Administrator" )]
public class AdminController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    
    public AdminController(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }
    
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Roles()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateNewRole(ApplicationRole role)
    {
        if (!ModelState.IsValid) return View(nameof(Roles), role);
        
        try
        {
            role.Id = Guid.NewGuid().ToString();
            if (role.Name != null) role.NormalizedName = role.Name.ToUpper();

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Roles));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return View(nameof(Roles), role);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        // if role is fixed, do not delete
        if (role.IsFixed) return RedirectToAction(nameof(Roles));
        
        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction(nameof(Roles));
        }

        return BadRequest();
    }
}