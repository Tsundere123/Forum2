using Forum2.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum2.DAL;

public class ForumThreadRepository : IForumThreadRepository
{
    private readonly ForumDbContext _db;
    private readonly ILogger<ForumThreadRepository> _logger;

    public ForumThreadRepository(ForumDbContext db, ILogger<ForumThreadRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<ForumThread>?> GetAll()
    {
        try
        {
            return await _db.ForumThread.ToListAsync();
        } 
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumThreadRepository] ForumThread GetAll failed, error message: {E}", e.Message);
            return null;
        }
    }
    
    public async Task<ForumThread?> GetForumThreadById(int id)
    {
        try
        {
            return await _db.ForumThread.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumThreadRepository] ForumThread GetForumThreadById failed, error message: {E}", e.Message);
            return null;
        }
    }

    public async Task<IEnumerable<ForumThread>?> GetForumThreadsByCategoryId(int id)
    {
        try
        {
            var threadList = await _db.ForumThread.ToListAsync();
            List<ForumThread> returnList = new List<ForumThread>();
            foreach(var forumThread in threadList)
            {
                if (forumThread.CategoryId == id)
                {
                    returnList.Add(forumThread);
                }
            }
            return returnList;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumThreadRepository] ForumThread GetForumThreadsByCategoryId failed, error message: {E}", e.Message);
            return null;
        }
    }
    
    public async Task<IEnumerable<ForumThread>?> GetForumThreadsByAccountId(string accountId)
    {
        try
        {
            var threadList = await _db.ForumThread.ToListAsync();
            List<ForumThread> returnList = new List<ForumThread>();
            foreach(var forumThread in threadList)
            {
                if (forumThread.CreatorId == accountId)
                {
                    returnList.Add(forumThread);
                }
            }
            return returnList;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumThreadRepository] ForumThread GetForumThreadsByAccountId failed, error message: {E}", e.Message);
            return null;
        }
    }

    public async Task<bool> CreateNewForumThread(ForumThread forumThread)
    {
        try
        {
            _db.ForumThread.Add(forumThread);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumThreadRepository] ForumThread CreateNewForumThread failed, error message: {E}", e.Message);
            return false;
        }
    }
    
    public async Task<bool> UpdateForumThread(ForumThread forumThread)
    {
        try
        {
            _db.ForumThread.Update(forumThread);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumThreadRepository] ForumThread UpdateForumThread failed, error message: {E}", e.Message);
            return false;
        }
    }
    
    public async Task<bool> DeleteForumThread(int id)
    {
        try
        {
            var forumThread = await _db.ForumThread.FindAsync(id);
            if (forumThread == null)
            {
                _logger.LogError("[ForumThreadRepository] ForumThread DeleteForumThread failed, forumThread with id {ID} not found", id);
                return false;
            }

            _db.ForumThread.Remove(forumThread);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumThreadRepository] ForumThread DeleteForumThread failed, error message: {E}", e.Message);
            return false;
        }
    }
}