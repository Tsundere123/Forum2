using Microsoft.EntityFrameworkCore;
using Forum2.Models;
using Forum2.ViewModels;

namespace Forum2.DAL;

public class ForumPostRepository : IForumPostRepository
{
    private readonly ForumDbContext _db;

    public ForumPostRepository(ForumDbContext db)
    {
        _db = db;
    }
        
    
    public async Task<IEnumerable<ForumPost>> GetAll()
    {
        return await _db.ForumPost.ToListAsync();
    }

    public async Task<ForumPost?> GetForumPostById(int id)
    {
        return await _db.ForumPost.FindAsync(id);
    }

    public async Task<IEnumerable<ForumPost>> GetAllForumPostsByThreadId(int threadId)
    {
        var postList = await _db.ForumPost.ToListAsync();
        List<ForumPost> returnList = new List<ForumPost>();
        foreach (var forumPost in postList)
        {
            if (forumPost.ForumThreadId == threadId)
            {
                returnList.Add(forumPost);
            }
        }
        return returnList;
    }
    
    public async Task<IEnumerable<ForumPost>> GetAllForumPostsByAccountId(string accountId)
    {
        var postList = await _db.ForumPost.ToListAsync();
        List<ForumPost> returnList = new List<ForumPost>();
        foreach (var forumPost in postList)
        {
            if (forumPost.ForumPostCreatorId == accountId)
            {
                returnList.Add(forumPost);
            }
        }
        return returnList;
    }

    public async Task CreateNewForumPost(ForumPost forumPost)
    {
        _db.ForumPost.Add(forumPost);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateForumPost(ForumPost forumPost)
    {
        _db.ForumPost.Update(forumPost);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> DeleteForumPost(ForumPost forumPost)
    {
        var forumThread = await _db.ForumPost.FindAsync(forumPost);
        if (forumPost == null)
        {
            return false;
        }

        _db.ForumPost.Remove(forumPost);
        await _db.SaveChangesAsync();
        return true;
    }
}