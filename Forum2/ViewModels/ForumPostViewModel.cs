using Forum2.Models;

namespace Forum2.ViewModels;


public class ForumPostViewModel
{
    public IEnumerable<ApplicationUser> Accounts;
    public ForumCategory ForumCategory;
    public ForumThread CurrentForumThread;
    public IEnumerable<ForumPost> ForumPosts;
    public int CurrentPage;
    public int TotalPages;

    public ForumPostViewModel(ForumCategory forumCategory, ForumThread currentForumThread,IEnumerable<ForumPost> forumPosts,
        IEnumerable<ApplicationUser> applicationUsers, int currentPage, int totalPages)
    {
        ForumCategory = forumCategory;
        CurrentForumThread = currentForumThread;
        ForumPosts = forumPosts;
        Accounts = applicationUsers;
        CurrentPage = currentPage;
        TotalPages = totalPages;
    }
}