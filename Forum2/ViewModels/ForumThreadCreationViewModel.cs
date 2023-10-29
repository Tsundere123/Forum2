using Forum2.Models;

namespace Forum2.ViewModels;

public class ForumThreadCreationViewModel
{
    public ForumCategory ForumCategory { get; set; }
    public ForumThread ForumThread { get; set; }
    public ForumPost ForumPost { get; set; }
}