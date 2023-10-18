using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumPostCreationViewModel
{
    public IEnumerable<ApplicationUser> Accounts;
    public ForumThread ForumThread;
    public ForumPost ForumPost;

    public ForumPostCreationViewModel(ForumThread forumThread,
        ForumPost forumPosts,
        IEnumerable<ApplicationUser> applicationUsers)
    {
        ForumThread = forumThread;
        ForumPost = forumPosts;
        Accounts = applicationUsers;
    }
}