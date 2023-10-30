using Microsoft.AspNetCore.Mvc;
using Forum2.DAL;

namespace Forum2.Controllers;

public class ForumCategoryController : Controller
{
    private readonly IForumCategoryRepository _forumCategoryRepository;
    
    public ForumCategoryController(IForumCategoryRepository forumCategoryRepository)
    {
        _forumCategoryRepository = forumCategoryRepository;
    }
    [Route("/Category")]
    public async Task<IActionResult> ForumCategoryTable()
    {
        var categories = await _forumCategoryRepository.GetAll();
        return View(categories);
    }
}