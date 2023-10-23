using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels;


public class ForumPostViewModel
{
    public IEnumerable<ApplicationUser> Accounts;
    public ForumCategory ForumCategory;
    public ForumThread CurrentForumThread;
    public IEnumerable<ForumPost> ForumPosts;

    public ForumPostViewModel(ForumCategory forumCategory, ForumThread currentForumThread,IEnumerable<ForumPost> forumPosts,
        IEnumerable<ApplicationUser> applicationUsers)
    {
        ForumCategory = forumCategory;
        CurrentForumThread = currentForumThread;
        ForumPosts = forumPosts;
        Accounts = applicationUsers;
    }
}