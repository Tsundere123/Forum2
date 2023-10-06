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
        // public string? CurrentViewName;

        public ForumListViewModel(IEnumerable<ForumCategory> forumCategories,IEnumerable<ForumThread> forumThreads)
        {
            ForumCategories = forumCategories;
            ForumThreads = forumThreads;
            // CurrentViewName = currentViewName;
        }
    }
}