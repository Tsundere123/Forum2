using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels;


public class ForumPostViewModel
{
    public IEnumerable<ApplicationUser> Accounts;
    public IEnumerable<ForumCategory> ForumCategories;
    public ForumThread CurrentForumThread;
    public IEnumerable<ForumPost> ForumPosts;

    public ForumPostViewModel(IEnumerable<ForumCategory> forumCategories, ForumThread currentForumThread,IEnumerable<ForumPost> forumPosts,
        IEnumerable<ApplicationUser> applicationUsers)
    {
        ForumCategories = forumCategories;
        CurrentForumThread = currentForumThread;
        ForumPosts = forumPosts;
        Accounts = applicationUsers;
    }
}