using Forum2.Models;

namespace Forum2.ViewModels;

public class ProfileIndexViewModel
{
    public ApplicationUser User { get; set; } = default!;
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public List<WallPost>? WallPosts { get; set; }
}