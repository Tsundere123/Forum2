using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumPostViewModel
{
    public ForumThread CurrentForumThread { get; set; }
    public IEnumerable<ForumPost> ForumPosts { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}