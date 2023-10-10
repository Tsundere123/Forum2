using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels
{
    public class ForumCategoryViewModel
    {
        public IEnumerable<ForumCategory> ForumCategories;
        public IEnumerable<ForumThread> ForumThreads;
        public string? CurrentViewName;

        public ForumCategoryViewModel(IEnumerable<ForumCategory> forumCategories, IEnumerable<ForumThread> forumThreads, string? currentViewName)
        {
            ForumCategories = forumCategories;
            ForumThreads = forumThreads;
            CurrentViewName = currentViewName;
        }
    }
}