using Forum2.Models;

namespace Forum2.ViewModels;

public class ProfileViewModel
{
    public ApplicationUser User { get; set; }
    public List<ApplicationRole>? Roles { get; set; }
    public List<ForumThread>? Threads { get; set; }
    public List<ForumPost>? Posts { get; set; }
}