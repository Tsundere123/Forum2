using Forum2.Models;

namespace Forum2.ViewModels;

public class HomeViewModel
{
    public List<ForumThread>? ForumThreads { get; set; }
    public List<ForumPost>? ForumPosts { get; set; }
}