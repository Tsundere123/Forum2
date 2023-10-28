using Forum2.DAL;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum2.Controllers;

[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IForumCategoryRepository _forumCategoryRepository;

    public AdminController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, 
        IForumCategoryRepository forumCategoryRepository)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _forumCategoryRepository = forumCategoryRepository;
    }
    
    //
    // Dashboard
    //
    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    //
    // Roles
    //
    
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
    
    //
    // Users
    //
    
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
        var user = await _userManager.FindByIdAsync(id);
        var roles = await _roleManager.Roles.ToListAsync();
        var userRoles = await _userManager.GetRolesAsync(user);
        var viewModel = new EditUserViewModel
        {
            User = user,
            Roles = roles,
            UserRoles = userRoles
        };
        return View(viewModel);
    }

    [HttpPost]
    [Route("/Admin/Users/Edit/{id}")]
    public async Task<IActionResult> EditUser(string id, EditUserViewModel viewModel)
    {
        if (!ModelState.IsValid) return View(viewModel);

        try
        {
            // User
            var user = await _userManager.FindByIdAsync(id);
            
            if (viewModel.User != null)
            {
                user.DisplayName = viewModel.User.DisplayName;
                user.Avatar = viewModel.User.Avatar;
            }

            var resultUser = await _userManager.UpdateAsync(user);
            if (!resultUser.Succeeded) return View(viewModel);

            // Roles
            var userRoles = await _userManager.GetRolesAsync(user);
            // Prevent a null reference exception if the user is assigned 0 roles
            viewModel.UserRoles ??= new List<string>();
            
            var rolesToRemove = userRoles.Except(viewModel.UserRoles).ToList();
            var rolesToAdd = viewModel.UserRoles.Except(userRoles).ToList();
            
            var resultRemoveRoles = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            var resultAddRoles = await _userManager.AddToRolesAsync(user, rolesToAdd);
            
            if (!resultRemoveRoles.Succeeded || !resultAddRoles.Succeeded) return View(viewModel);

            return RedirectToAction(nameof(Users));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    //
    // Categories
    //
    
    [HttpGet]
    public async Task<IActionResult> Categories()
    {
        var categories = await _forumCategoryRepository.GetAll();
        return View(categories);
    }
    
    [HttpGet]
    [Route("/Admin/Categories/New")]
    public IActionResult NewCategory()
    {
        return View();
    }

    [HttpPost]
    [Route("/Admin/Categories/New")]
    public async Task<IActionResult> NewCategory(ForumCategory category)
    {
        if (!ModelState.IsValid) return View(category);

        try
        {
            await _forumCategoryRepository.CreateForumCategory(category);
            return RedirectToAction(nameof(Categories));
        } 
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    [Route("/Admin/Categories/Edit/{id}")]
    public async Task<IActionResult> EditCategory(int id)
    {
        var category = await _forumCategoryRepository.GetForumCategoryById(id);
        return View(category);
    }

    [HttpPost]
    [Route("/Admin/Categories/Edit/{id}")]
    public async Task<IActionResult> EditCategory(int id, ForumCategory category)
    {
        if (!ModelState.IsValid) return View(category);

        try
        {
            category.Id = id;
            await _forumCategoryRepository.UpdateForumCategory(category);
            return RedirectToAction(nameof(Categories));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    [Route("/Admin/Categories/Delete/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _forumCategoryRepository.GetForumCategoryById(id);
        return View(category);
    }

    [HttpPost]
    [Route("/Admin/Categories/Delete/{id}")]
    public async Task<IActionResult> DeleteCategory(int id, ForumCategory category)
    {
        try
        {
            var categoryToDelete = await _forumCategoryRepository.GetForumCategoryById(id);
            if (categoryToDelete == null) return NotFound();
            
            await _forumCategoryRepository.DeleteForumCategory(categoryToDelete);
            return RedirectToAction(nameof(Categories));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}