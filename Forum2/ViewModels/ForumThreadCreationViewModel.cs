using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumThreadCreationViewModel
{
    public IEnumerable<ApplicationUser> Accounts;
    public ForumCategory ForumCategory;
    public ForumThread ForumThread;
    public ForumPost ForumPost;

    public ForumThreadCreationViewModel(ForumCategory forumCategory, ForumThread forumThread,
        ForumPost forumPost,
        IEnumerable<ApplicationUser> applicationUsers)
    {
        ForumCategory = forumCategory;
        ForumThread = forumThread;
        ForumPost = forumPost;
        Accounts = applicationUsers;
    }

}