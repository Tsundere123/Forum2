using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumListViewModel
{
    public IEnumerable<ForumThread> ForumThreads { get; set; }
    public IEnumerable<ForumCategory> ForumCategories { get; set; }
    public IEnumerable<ApplicationUser> Accounts { get; set; }
}