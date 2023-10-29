using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumCategoryViewModel
{
    public IEnumerable<ForumCategory> ForumCategories { get; set; }
    public IEnumerable<ForumThread> ForumThreads { get; set; }
}