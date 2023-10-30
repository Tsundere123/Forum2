using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumThreadCreationViewModel
{
    public ForumCategory ForumCategory { get; set; } = default!;
    public ForumThread ForumThread { get; set; } = default!;
    public ForumPost ForumPost { get; set; } = default!;
}