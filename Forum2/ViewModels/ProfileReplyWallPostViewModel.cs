using Forum2.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Forum2.ViewModels;

public class ProfileReplyWallPostViewModel
{
    [ValidateNever]
    public ApplicationUser User { get; set; } = default!;
    
    [ValidateNever]
    public int WallPostId { get; set; }
    
    public string Content { get; set; } = string.Empty;
}