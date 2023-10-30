using Forum2.Models;

namespace Forum2.ViewModels;

public class ProfileThreadsViewModel
{
    public ApplicationUser User { get; set; } = default!;
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public List<ForumThread>? Threads { get; set; }
}