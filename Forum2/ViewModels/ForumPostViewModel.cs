using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels;


public class ForumPostViewModel
{
    public IEnumerable<ApplicationUser> Accounts;
    public IEnumerable<ForumCategory> ForumCategories;
    public IEnumerable<ForumThread> ForumThreads;
    public IEnumerable<ForumPost> ForumPosts;

    public ForumPostViewModel(IEnumerable<ForumCategory> forumCategories, IEnumerable<ForumThread> forumThreads,IEnumerable<ForumPost> forumPosts,
        IEnumerable<ApplicationUser> applicationUsers)
    {
        ForumCategories = forumCategories;
        ForumThreads = forumThreads;
        ForumPosts = forumPosts;
        Accounts = applicationUsers;
    }
}