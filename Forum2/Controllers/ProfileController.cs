using Forum2.DAL;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

[Route("Profile/{displayName}")]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;
    private readonly IWallPostRepository _forumWallPostRepository;
    private readonly IWallPostReplyRepository _forumWallPostReplyRepository;

    public ProfileController(UserManager<ApplicationUser> userManager,
        IForumThreadRepository forumThreadRepository, IForumPostRepository forumPostRepository,
        IWallPostRepository forumWallPostRepository, IWallPostReplyRepository forumWallPostReplyRepository)
    {
        _userManager = userManager;
        _forumThreadRepository = forumThreadRepository;
        _forumPostRepository = forumPostRepository;
        _forumWallPostRepository = forumWallPostRepository;
        _forumWallPostReplyRepository = forumWallPostReplyRepository;
    }

    // For consistency, use the same number of items per page for all lists
    private const int CountPerPage = 10;

    //
    // Profile Wall
    // 
    
    // GET list of wall posts
    [HttpGet]
    [Route("{page?}")]
    public async Task<IActionResult> Index(string displayName, int? page)
    {
        if (string.IsNullOrEmpty(displayName)) return NotFound();
        
        var user = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (user == null) return NotFound();
        
        var wallPosts = await _forumWallPostRepository.GetAllByProfile(user.Id);
        var postsCount = (wallPosts ?? Array.Empty<WallPost>()).Count();
        var totalPages = (int) Math.Ceiling((double) postsCount / CountPerPage);
        var currentPage = page ?? 1;
        var postsToShow = (wallPosts ?? Array.Empty<WallPost>()).Skip((currentPage - 1) * CountPerPage).Take(CountPerPage).ToList();

        var model = new ProfileIndexViewModel()
        {
            User = user,
            CurrentPage = currentPage,
            TotalPages = totalPages,
            WallPosts = postsToShow
        };
        return View(model);
    }

    // GET new wall post form
    [HttpGet]
    [Route("New")]
    [Authorize]
    public Task<IActionResult> NewWallPost(string displayName)
    {
        if (string.IsNullOrEmpty(displayName)) return Task.FromResult<IActionResult>(NotFound());
        
        var user = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (user == null) return Task.FromResult<IActionResult>(NotFound());
        
        var model = new ProfileNewWallPostViewModel
        {
            User = user
        };
        return Task.FromResult<IActionResult>(View(model));
    }
    
    // POST new wall post
    [HttpPost]
    [Route("New")]
    [Authorize]
    public async Task<IActionResult> NewWallPost(string displayName, ProfileNewWallPostViewModel model)
    {
        if (string.IsNullOrEmpty(displayName)) return NotFound();
        
        var profileUser = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (profileUser == null) return NotFound();

        if (!ModelState.IsValid)
        {
            model.User = profileUser;
            return View(model);
        }
        
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null) return NotFound();
        
        var wallPost = new WallPost
        {
            AuthorId = user.Id,
            ProfileId = profileUser.Id,
            Content = model.Content,
            CreatedAt = DateTime.Now
        };
        
        var result = await _forumWallPostRepository.Create(wallPost);
        if (!result) return StatusCode(500);
        
        return RedirectToAction("Index", new {displayName});
    }
    
    // GET reply to wall post form
    [HttpGet]
    [Route("Reply/{id}")]
    [Authorize]
    public async Task<IActionResult> ReplyWallPost(string displayName, int id)
    {
        if (string.IsNullOrEmpty(displayName)) return NotFound();
        
        var wallPost = await _forumWallPostRepository.GetById(id);
        if (wallPost == null) return NotFound();
        
        var user = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (user == null) return NotFound();
        
        var model = new ProfileReplyWallPostViewModel()
        {
            User = user, 
            WallPostId = id
        };
        return View(model);
    }

    // POST reply to wall post
    [HttpPost]
    [Route("Reply/{id}")]
    [Authorize]
    public async Task<IActionResult> ReplyWallPost(string displayName, int id, ProfileReplyWallPostViewModel model)
    {
        if (string.IsNullOrEmpty(displayName)) return NotFound();
        
        var wallPost = await _forumWallPostRepository.GetById(id);
        if (wallPost == null) return NotFound();
        
        var profileUser = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (profileUser == null) return NotFound();
        
        if (!ModelState.IsValid)
        {
            model.User = profileUser;
            model.WallPostId = id;
            return View(model);
        }
        
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null) return NotFound();

        var wallPostReply = new WallPostReply
        {
            AuthorId = user.Id,
            WallPostId = id,
            Content = model.Content
        };
        
        var result = await _forumWallPostReplyRepository.Create(wallPostReply);
        if (!result) return StatusCode(500);
        
        return RedirectToAction("Index", new {displayName});
    }

    // POST delete wall post
    [HttpPost]
    [Authorize]
    [Route("/Profile/{displayName}/Delete/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteWallPost(string displayName, int id)
    {
        if (string.IsNullOrEmpty(displayName)) return NotFound();
        
        var wallPost = await _forumWallPostRepository.GetById(id);
        if (wallPost == null) return NotFound();
        
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var profileUser = await _userManager.FindByIdAsync(wallPost.ProfileId);

        if (HttpContext.User.IsInRole("Moderator") || HttpContext.User.IsInRole("Administrator") || user.Id == wallPost.AuthorId || user.Id == profileUser.Id)
        {
            var result = await _forumWallPostRepository.Delete(id);
            if (!result) return BadRequest();
            return RedirectToAction("Index", new {displayName});
        }

        return StatusCode(500);
    }

    // POST delete wall post reply
    [HttpPost]
    [Authorize]
    [Route("/Profile/{displayName}/DeleteReply/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteWallPostReply(string displayName, int id)
    {
        if (string.IsNullOrEmpty(displayName)) return NotFound();
        
        var wallPostReply = await _forumWallPostReplyRepository.GetById(id);
        if (wallPostReply == null) return NotFound();
        
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var profileUser = await _userManager.FindByIdAsync(wallPostReply.WallPost.ProfileId);
        
        if (HttpContext.User.IsInRole("Moderator") || HttpContext.User.IsInRole("Administrator") || user.Id == wallPostReply.AuthorId || user.Id == profileUser.Id)
        {
            var result = await _forumWallPostReplyRepository.Delete(id);
            if (!result) return BadRequest();
            return RedirectToAction("Index", new {displayName});
        }
        
        return StatusCode(500);
    }
    
    //
    // Profile Threads
    // 
    
    // GET list of threads
    [HttpGet]
    [Route("/Profile/{displayName}/Threads/{page?}")]
    public async Task<IActionResult> Threads(string displayName, int? page)
    {
        if (string.IsNullOrEmpty(displayName)) return NotFound();
        
        var user = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (user == null) return NotFound();
        
        var threads = await _forumThreadRepository.GetForumThreadsByAccountId(user.Id);
        var threadsCount = (threads ?? Array.Empty<ForumThread>()).Count();
        var totalPages = (int) Math.Ceiling((double) threadsCount / CountPerPage);
        var currentPage = page ?? 1;
        var threadsToShow = (threads ?? Array.Empty<ForumThread>()).Skip((currentPage - 1) * CountPerPage).Take(CountPerPage).ToList();
        
        var model = new ProfileThreadsViewModel
        {
            User = user,
            CurrentPage = currentPage,
            TotalPages = totalPages,
            Threads = threadsToShow
        };
        return View(model);
    }
    
    
    //
    // Profile Posts
    // 
    
    // GET list of posts
    [HttpGet()]
    [Route("/Profile/{displayName}/Posts/{page?}")]
    public async Task<IActionResult> Posts(string displayName, int? page)
    {
        if (string.IsNullOrEmpty(displayName)) return NotFound();
        
        var user = _userManager.Users.FirstOrDefault(u => u.DisplayName == displayName);
        if (user == null) return NotFound();
        
        var posts = await _forumPostRepository.GetAllForumPostsByAccountId(user.Id);
        var postsCount = (posts ?? Array.Empty<ForumPost>()).Count();
        var totalPages = (int) Math.Ceiling((double) postsCount / CountPerPage);
        var currentPage = page ?? 1;
        var postsToShow = (posts ?? Array.Empty<ForumPost>()).Skip((currentPage - 1) * CountPerPage).Take(CountPerPage).ToList();
        
        var model = new ProfilePostsViewModel
        {
            User = user,
            CurrentPage = currentPage,
            TotalPages = totalPages,
            Posts = postsToShow
        };
        return View(model);
    }
}