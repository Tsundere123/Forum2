using System;
using System.Collections;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels
{
    public class ForumListViewModel
    {
        public IEnumerable<ForumThread> ForumThreads;
        public IEnumerable<ForumCategory> ForumCategories;
        public IEnumerable<ApplicationUser> Accounts;
        // public string? CurrentViewName;

        public ForumListViewModel(IEnumerable<ForumCategory> forumCategories,IEnumerable<ForumThread> forumThreads, IEnumerable<ApplicationUser> accounts)
        {
            ForumCategories = forumCategories;
            ForumThreads = forumThreads;
            Accounts = accounts;
            // CurrentViewName = currentViewName;
        }
    }
}