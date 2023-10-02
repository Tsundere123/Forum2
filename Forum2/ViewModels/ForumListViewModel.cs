using System;
using System.Collections.Generic;
using Forum2.Models;

namespace Forum2.ViewModels
{
    public class ForumListViewModel
    {
        public IEnumerable<ForumThread> ForumThreads;
        public string? CurrentViewName;

        public ForumListViewModel(IEnumerable<ForumThread> forumThreads, string? currentViewName)
        {
            ForumThreads = forumThreads;
            CurrentViewName = currentViewName;
        }
    }
}