using Forum2.Models;

namespace Forum2.ViewModels;

public class ProfilePostsViewModel
{
    public ApplicationUser User { get; set; } = default!;
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public List<ForumPost>? Posts { get; set; }
}