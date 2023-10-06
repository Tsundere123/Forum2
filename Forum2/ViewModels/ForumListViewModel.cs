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

        public IEnumerable<Account> Accounts;
        // public string? CurrentViewName;

        public ForumListViewModel(IEnumerable<ForumCategory> forumCategories,IEnumerable<ForumThread> forumThreads, IEnumerable<Account> accounts)
        {
            ForumCategories = forumCategories;
            ForumThreads = forumThreads;
            Accounts = accounts;
            // CurrentViewName = currentViewName;
        }
    }
}