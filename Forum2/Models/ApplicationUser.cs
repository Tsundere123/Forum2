using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Forum2.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MinLength(2)]
    public string DisplayName { get; set; } = string.Empty;
    
    [Required]
    public string? AvatarUrl { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}