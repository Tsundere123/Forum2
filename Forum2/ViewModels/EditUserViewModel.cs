using Forum2.Models;

namespace Forum2.ViewModels;

public class EditUserViewModel
{
    public ApplicationUser? User { get; set; }
    public List<ApplicationRole>? Roles { get; set; }
    public IList<string>? UserRoles { get; set; }
}