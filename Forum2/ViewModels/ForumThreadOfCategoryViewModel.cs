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
        public int CurrentPage;
        public int TotalPages;

        public IEnumerable<ApplicationUser> Accounts;
        // public string? CurrentViewName;

        public ForumThreadOfCategoryViewModel(ForumCategory forumCategory,IEnumerable<ForumThread> forumThreads, IEnumerable<ApplicationUser> accounts, int currentPage, int totalPages)
        {
            ForumCategory = forumCategory;
            ForumThreads = forumThreads;
            Accounts = accounts;
            CurrentPage = currentPage;
            TotalPages = totalPages;
            // CurrentViewName = currentViewName;
        }
    }
}