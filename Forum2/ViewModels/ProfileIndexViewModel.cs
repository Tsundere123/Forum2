using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Forum2.Models;

namespace Forum2.ViewModels;

public class ProfileIndexViewModel
{
    public ApplicationUser User { get; set; }
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public List<WallPost>? WallPosts { get; set; }
    
    [DisplayName("new wall post content")] // Lowercase for validation error message
    public string NewWallPostContent { get; set; } = string.Empty;
    
    [DisplayName("reply content")] // Lowercase for validation error message
    public string NewWallPostContentReply { get; set; } = string.Empty;
}