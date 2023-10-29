using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumThreadOfCategoryViewModel
{
    public List<ForumThread>? PinnedThreads { get; set; }
    public List<ForumThread>? ForumThreads { get; set; }
    public ForumCategory ForumCategory { get; set; } = new ();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}