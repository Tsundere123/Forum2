using Forum2.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum2.DAL;

public class ForumThreadRepository : IForumThreadRepository
{
    private readonly ForumDbContext _db;

    public ForumThreadRepository(ForumDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ForumThread>> GetAll()
    {
        return await _db.ForumThread.ToListAsync();
    }

    public async Task<ForumThread?> GetForumThreadById(int id)
    {
        return await _db.ForumThread.FindAsync(id);
    }

    public async Task CreateNewForumThread(ForumThread forumThread)
    {
        _db.ForumThread.Add(forumThread);
        await _db.SaveChangesAsync();
    }
    
    public async Task UpdateForumThread(ForumThread forumThread)
    {
        _db.ForumThread.Update(forumThread);
        await _db.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteForumThread(int id)
    {
        var forumThread = await _db.ForumThread.FindAsync(id);
        if (forumThread == null)
        {
            return false;
        }

        _db.ForumThread.Remove(forumThread);
        await _db.SaveChangesAsync();
        return true;
    }
}