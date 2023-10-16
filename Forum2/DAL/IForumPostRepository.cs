using Forum2.Models;

namespace Forum2.DAL;

public interface IForumPostRepository
{
    Task<IEnumerable<ForumPost>> GetAll();
    Task<ForumPost> GetForumPostById(int id);
    Task<IEnumerable<ForumPost>> GetAllForumPostsByThreadId(int threadId);
    Task<IEnumerable<ForumPost>> GetAllForumPostsByAccountId(string accountId);
    
    Task CreateForumPost(ForumPost forumPost);
    Task UpdateForumPost(ForumPost forumPost);
    Task<bool> DeleteForumPost(ForumPost forumPost);
    
}