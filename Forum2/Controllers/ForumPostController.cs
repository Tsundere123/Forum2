﻿using Forum2.DAL;
using Forum2.Models;
using Forum2.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Forum2.Controllers;

public class ForumPostController : Controller
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IForumCategoryRepository _forumCategoryRepository;
    private readonly IForumThreadRepository _forumThreadRepository;
    private readonly IForumPostRepository _forumPostRepository;

    public ForumPostController(IAccountRepository accountRepository, IAccountRoleRepository accountRoleRepository,
        IForumCategoryRepository forumCategoryRepository, IForumThreadRepository forumThreadRepository,IForumPostRepository forumPostRepository)
    {
        _forumCategoryRepository = forumCategoryRepository;
        _forumThreadRepository = forumThreadRepository;
        _accountRoleRepository = accountRoleRepository;
        _accountRepository = accountRepository;
        _forumPostRepository = forumPostRepository;
    }
    public async Task<IActionResult> ForumPostView(int threadId)
    {
        var forumPosts = await _forumPostRepository.GetAllForumPostsByThreadId(threadId);
        var forumCategories = await _forumCategoryRepository.GetAll();
        var forumThreads = await _forumThreadRepository.GetAll();
        var accounts = await _accountRepository.GetAll();
        var forumPostViewModel = new ForumPostViewModel(forumCategories, forumThreads, forumPosts, accounts);
        
        return View(forumPostViewModel);
    }
}