using Forum2.DAL;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;
    
    public ProfileController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IForumThreadRepository forumThreadRepository, IForumPostRepository forumPostRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _forumThreadRepository = forumThreadRepository;
        _forumPostRepository = forumPostRepository;
    }
    
    [HttpGet]
    [Route("Profile/{displayName}")]
    public async Task<IActionResult> Index(string displayName)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (user == null) return NotFound();
        var userRoles = await _userManager.GetRolesAsync(user);
        var roles = _roleManager.Roles.Where(r => userRoles.Contains(r.Name));
        
        var threads = await _forumThreadRepository.GetForumThreadsByAccountId(user.Id);
        var posts = await _forumPostRepository.GetAllForumPostsByAccountId(user.Id);
        var model = new ProfileViewModel
        {
            User = user,
            Roles = roles.ToList(),
            Threads = threads.ToList(),
            Posts = posts.ToList()
        };
        return View(model);
    }
}