using Forum2.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum2.DAL;

public class WallPostRepository : IWallPostRepository
{
    private readonly ForumDbContext _db;
    private readonly ILogger<WallPostRepository> _logger;
    
    public WallPostRepository(ForumDbContext db, ILogger<WallPostRepository> logger)
    {
        _db = db;
        _logger = logger;
    }
    
    public async Task<IEnumerable<WallPost>?> GetAllByProfile(string id)
    {
        try
        {
            return await _db.WallPost.Where(p => p.ProfileId == id).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[WallPostRepository] WallPost GetAllByProfile failed, error message: {E}", e.Message);
            return null;
        }
    }

    public async Task<WallPost?> GetById(int id)
    {
        try
        {
            return await _db.WallPost.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[WallPostRepository] WallPost GetWallPostById failed, error message: {E}", e.Message);
            return null;
        }
    }

    public async Task<bool> Create(WallPost wallPost)
    {
        try
        {
            _db.WallPost.Add(wallPost);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[WallPostRepository] WallPost CreateNewWallPost failed, error message: {E}", e.Message);
            return false;
        }
    }
    
    public async Task<bool> Delete(int wallPostId)
    {
        try
        {
            var wallPost = await _db.WallPost.FindAsync(wallPostId);
            if (wallPost == null)
            {
                _logger.LogError("[WallPostRepository] WallPost DeleteWallPost failed, wallPost with id {ID} not found", wallPostId);
                return false;
            }
            
            _db.WallPost.Remove(wallPost);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[WallPostRepository] WallPost DeleteWallPost failed, error message: {E}", e.Message);
            return false;
        }
    }
}