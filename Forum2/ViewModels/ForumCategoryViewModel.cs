using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels
{
    public class ForumCategoryViewModel
    {
        public IEnumerable<ForumCategory> ForumCategories;
        public string? CurrentViewName;

        public ForumCategoryViewModel(IEnumerable<ForumCategory> forumCategories, string? currentViewName)
        {
            ForumCategories = forumCategories;
            CurrentViewName = currentViewName;
        }
    }
}