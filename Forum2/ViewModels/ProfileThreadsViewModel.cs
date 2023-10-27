using Forum2.Models;

namespace Forum2.ViewModels;

public class ProfileThreadsViewModel
{
    public ApplicationUser User { get; set; }
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public List<ForumThread>? Threads { get; set; }
}