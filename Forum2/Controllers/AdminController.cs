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
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;
    private readonly ILogger<AdminController> _logger;
    
    public AdminController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, 
        IForumCategoryRepository forumCategoryRepository, IForumThreadRepository forumThreadRepository, 
        IForumPostRepository forumPostRepository, ILogger<AdminController> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _forumCategoryRepository = forumCategoryRepository;
        _forumThreadRepository = forumThreadRepository;
        _forumPostRepository = forumPostRepository;
        _logger = logger;
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
        
        var roleExists = await _roleManager.RoleExistsAsync(role.Name);
        if (roleExists)
        {
            ModelState.AddModelError("Name", "Role already exists");
            return View(role);
        }

        role.Id = Guid.NewGuid().ToString();
        if (role.Name != null) role.NormalizedName = role.Name.ToUpper();

        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded)
        { 
            return RedirectToAction(nameof(Roles));
        }
        
        _logger.LogError("[AdminController] Unknown error creating role: {R})", role.Name);
        return StatusCode(500);
    }

    [HttpGet]
    [Route("/Admin/Roles/Edit/{id}")]
    public async Task<IActionResult> EditRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    [HttpPost]
    [Route("/Admin/Roles/Edit/{id}")]
    public async Task<IActionResult> EditRole(string id, ApplicationRole role)
    {
        if (!ModelState.IsValid) return View(role);

        var roleToUpdate = await _roleManager.FindByIdAsync(id);
        var roleWithName = await _roleManager.FindByNameAsync(role.Name);
        
        if (roleWithName != null && roleWithName.Id != roleToUpdate.Id)
        {
            ModelState.AddModelError("Name", "Role already exists with that name");
            return View(role);
        }
        
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
        
        Console.Write(result.Errors);
        
        _logger.LogError("[AdminController] Unknown error updating role: {R})", role.Name);
        return StatusCode(500);
    }

    [HttpGet]
    [Route("/Admin/Roles/Delete/{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    [HttpPost]
    [Route("/Admin/Roles/Delete/{id}")]
    public async Task<IActionResult> DeleteRole(string id, ApplicationRole role)
    {
        var roleToDelete = await _roleManager.FindByIdAsync(id);
        if (!roleToDelete.IsFixed)
        { 
            var result = await _roleManager.DeleteAsync(roleToDelete);
            if (result.Succeeded)
            { 
                return RedirectToAction(nameof(Roles));
            }
            
            _logger.LogError("[AdminController] Unknown error deleting role: {R})", role.Name);
            return StatusCode(500);
        }

        return BadRequest();
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
        if (user == null) return NotFound();
        
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

        // User
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
            
        if (viewModel.User != null)
        { 
            user.DisplayName = viewModel.User.DisplayName; 
            user.Avatar = viewModel.User.Avatar;
        }

        var resultUser = await _userManager.UpdateAsync(user);
        if (!resultUser.Succeeded) return BadRequest();

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
    
    //
    // Categories
    //
    
    [HttpGet]
    public async Task<IActionResult> Categories()
    {
        var categories = await _forumCategoryRepository.GetAll();
        if (categories == null) return NotFound();
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

        var result = await _forumCategoryRepository.CreateForumCategory(category);
        if (result) return RedirectToAction(nameof(Categories));
        
        return StatusCode(500);
    }

    [HttpGet]
    [Route("/Admin/Categories/Edit/{id}")]
    public async Task<IActionResult> EditCategory(int id)
    {
        var category = await _forumCategoryRepository.GetForumCategoryById(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    [Route("/Admin/Categories/Edit/{id}")]
    public async Task<IActionResult> EditCategory(int id, ForumCategory category)
    {
        if (!ModelState.IsValid) return View(category);

        category.Id = id;
        var result = await _forumCategoryRepository.UpdateForumCategory(category);
        if (result) return RedirectToAction(nameof(Categories));

        return StatusCode(500);
    }

    [HttpGet]
    [Route("/Admin/Categories/Delete/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _forumCategoryRepository.GetForumCategoryById(id);
        if (category == null) return NotFound();
        
        return View(category);
    }

    [HttpPost]
    [Route("/Admin/Categories/Delete/{id}")]
    public async Task<IActionResult> DeleteCategory(int id, ForumCategory category)
    {
        var categoryToDelete = await _forumCategoryRepository.GetForumCategoryById(id);
        if (categoryToDelete == null) return NotFound();
            
        var result = await _forumCategoryRepository.DeleteForumCategory(categoryToDelete);
        if (result) return RedirectToAction(nameof(Categories));
        return StatusCode(500);
    }
    
    [HttpGet]
    [Route("/ForumThread/Admin/Threads")]
    public async Task<IActionResult> ViewAllSoftDeletedThreads()
    {
        var allThreads = await _forumThreadRepository.GetAll();
        if (allThreads == null) return View(new List<ForumThread>());
        var allSoftDeletedThreads = allThreads.Where(thread => thread.IsSoftDeleted).ToList();

        return View(allSoftDeletedThreads);
    }
    
    [HttpGet]
    [Route("/ForumThread/Admin/Posts")]
    public async Task<IActionResult> ViewAllSoftDeletedPosts()
    {
        var allPosts = await _forumPostRepository.GetAll();
        if (allPosts == null) return View(new List<ForumPost>());
        var allSoftDeletedPosts = allPosts.Where(post => post.IsSoftDeleted).ToList();

        return View(allSoftDeletedPosts);
    }
}