using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumPostCreationViewModel
{
    public ForumThread ForumThread { get; set; } = default!;
    public ForumPost ForumPost { get; set; } = default!;
}