using Microsoft.AspNetCore.Identity;

namespace Forum2.Models;

public class ApplicationRole : IdentityRole<string>
{
    public string? Color { get; set; } = string.Empty;
    public bool IsFixed { get; set; } = false;
}