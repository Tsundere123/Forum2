using Forum2.Models;

namespace Forum2.DAL;

public interface IForumCategoryRepository
{
    Task<IEnumerable<ForumCategory>> GetAll();
    Task<ForumCategory?> GetForumCategoryById(int id);
    Task CreateForumCategory(ForumCategory forumCategory);
    Task UpdateForumCategory(ForumCategory forumCategory);
}