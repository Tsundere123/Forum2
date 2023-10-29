using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumThreadOfCategoryViewModel
{
    public IEnumerable<ForumThread> ForumThreads { get; set; } = new List<ForumThread>();
    public ForumCategory ForumCategory { get; set; } = new ();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}