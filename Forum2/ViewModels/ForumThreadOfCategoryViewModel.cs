using System;
using System.Collections;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels
{
    public class ForumThreadOfCategoryViewModel
    {
        public IEnumerable<ForumThread> ForumThreads;
        public ForumCategory ForumCategory;

        public IEnumerable<ApplicationUser> Accounts;
        // public string? CurrentViewName;

        public ForumThreadOfCategoryViewModel(ForumCategory forumCategory,IEnumerable<ForumThread> forumThreads, IEnumerable<ApplicationUser> accounts)
        {
            ForumCategory = forumCategory;
            ForumThreads = forumThreads;
            Accounts = accounts;
            // CurrentViewName = currentViewName;
        }
    }
}