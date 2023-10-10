using Microsoft.AspNetCore.Identity;

namespace Forum2.Models;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; } = string.Empty;
}