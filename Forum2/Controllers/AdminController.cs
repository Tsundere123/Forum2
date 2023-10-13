using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace Forum2.Controllers;

[Authorize( Roles = "Administrator" )]
public class AdminController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public AdminController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }
    
    // Dashboard
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    // Roles
    [HttpGet]
    public IActionResult Roles()
    {
        return View();
    }
    
    [HttpGet]
    [Route("Admin/Roles/New")]
    public IActionResult NewRole()
    {
        return View();
    }
    
    [HttpPost]
    [Route("/Admin/Roles/New")]
    public async Task<IActionResult> NewRole(ApplicationRole role)
    {
        if (!ModelState.IsValid) return View(role);
        
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

        return View(role);
    }
    
    [HttpGet]
    [Route("/Admin/Roles/Edit/{id}")]
    public async Task<IActionResult> EditRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        return View(role);
    }
    
    [HttpPost]
    [Route("/Admin/Roles/Edit/{id}")]
    public async Task<IActionResult> EditRole(string id, ApplicationRole role)
    {
        if (!ModelState.IsValid) return View(role);
        
        try
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(id);
            if (!roleToUpdate.IsFixed)
            {
                roleToUpdate.Name = role.Name;
                roleToUpdate.NormalizedName = role.Name.ToUpper();
            }
            roleToUpdate.Color = role.Color;

            var result = await _roleManager.UpdateAsync(roleToUpdate);
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

        return View(role);
    }
    
    [HttpGet]
    [Route("/Admin/Roles/Delete/{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        return View(role);
    }
    
    [HttpPost]
    [Route("/Admin/Roles/Delete/{id}")]
    public async Task<IActionResult> DeleteRole(string id, ApplicationRole role)
    {
        try
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);
            if (!roleToDelete.IsFixed)
            {
                var result = await _roleManager.DeleteAsync(roleToDelete);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Roles));
                }
            }

            return BadRequest();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    // Users
    [HttpGet]
    public IActionResult Users()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Users(string username, string role)
    {
        var users = await _userManager.Users.ToListAsync();
        if (!string.IsNullOrEmpty(username))
        {
            users = users.Where(u => u.DisplayName.ToUpper().Contains(username.ToUpper())).ToList();
        }
        
        if (!string.IsNullOrEmpty(role) && role != "All")
        {
            var usersWithRole = await _userManager.GetUsersInRoleAsync(role);
            users = users.Where(u => usersWithRole.Contains(u)).ToList();
        }
        
        return View(users);
    }
    
    [HttpGet]
    [Route("/Admin/Users/Edit/{id}")]
    public async Task<IActionResult> EditUser(string id)
    {
        return View();
    }
}