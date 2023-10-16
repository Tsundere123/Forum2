using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumThreadCreationViewModel
{
    public IEnumerable<ApplicationUser> Accounts;
    public ForumCategory ForumCategory;
    public ForumThread CurrentForumThread;
    public ForumPost ForumPost;

    public ForumThreadCreationViewModel(ForumCategory forumCategory, ForumThread currentForumThread,
        ForumPost forumPosts,
        IEnumerable<ApplicationUser> applicationUsers)
    {
        ForumCategory = forumCategory;
        CurrentForumThread = currentForumThread;
        ForumPost = forumPosts;
        Accounts = applicationUsers;
    }

}