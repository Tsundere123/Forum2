using Forum2.Models;

namespace Forum2.ViewModels;

public class SearchResultViewModel
{
    public string? Query { get; set; }
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public List<ForumThread>? Threads { get; set; }
    public List<ForumPost>? Posts { get; set; }
    public List<ApplicationUser>? Users { get; set; }
}