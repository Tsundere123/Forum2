using Forum2.Models;

namespace Forum2.ViewModels;

public class ProfileViewModel
{
    public ApplicationUser User { get; set; }
    public List<ApplicationRole>? Roles { get; set; }
}