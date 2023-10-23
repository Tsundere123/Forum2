using Forum2.Models;

namespace Forum2.DAL;

public interface IForumCategoryRepository
{
    Task<IEnumerable<ForumCategory>?> GetAll();
    Task<ForumCategory?> GetForumCategoryById(int id);
    Task<bool> CreateForumCategory(ForumCategory forumCategory);
    Task<bool> UpdateForumCategory(ForumCategory forumCategory);
    Task<bool> DeleteForumCategory(ForumCategory forumCategory);
}