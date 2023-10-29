using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum2.DAL;

namespace Forum2.Controllers;

public class ForumCategoryController : Controller
{
    private readonly IForumCategoryRepository _forumCategoryRepository;
    private readonly IForumThreadRepository _forumThreadRepository;
    
    public ForumCategoryController(IForumCategoryRepository forumCategoryRepository, IForumThreadRepository forumThreadRepository)
    {
        _forumCategoryRepository = forumCategoryRepository;
        _forumThreadRepository = forumThreadRepository;
    }
    [Route("/Category")]
    public async Task<IActionResult> ForumCategoryTable()
    {
        var forumCategories = await _forumCategoryRepository.GetAll();
        var forumThreads = await _forumThreadRepository.GetAll();
        var forumCategoriesListViewModel = new ForumCategoryViewModel
        {
            ForumCategories = forumCategories,
            ForumThreads = forumThreads
        };
        return View(forumCategoriesListViewModel);
    }
}