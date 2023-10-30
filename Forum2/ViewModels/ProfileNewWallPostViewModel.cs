using Forum2.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Forum2.ViewModels;

public class ProfileNewWallPostViewModel
{
    [ValidateNever] public ApplicationUser User { get; set; } = default!;
    
    public string Content { get; set; } = string.Empty;
}