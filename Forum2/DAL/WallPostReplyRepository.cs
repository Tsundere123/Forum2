using Forum2.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum2.DAL;

public class WallPostReplyRepository : IWallPostReplyRepository
{
    private readonly ForumDbContext _db;
    private readonly ILogger<WallPostReplyRepository> _logger;
    
    public WallPostReplyRepository(ForumDbContext db, ILogger<WallPostReplyRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<WallPostReply>?> GetAll()
    {
        try
        {
            return await _db.WallPostReply.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[WallPostReplyRepository] WallPostReply GetAll failed, error message: {E}", e.Message);
            return null;
        }
    }

    public async Task<IEnumerable<WallPostReply>?> GetAllByWallPostId(int wallPostId)
    {
        try
        {
            return await _db.WallPostReply.Where(r => r.WallPostId == wallPostId).ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[WallPostReplyRepository] WallPostReply GetAllByWallPostId failed, error message: {E}", e.Message);
            return null;
        }
    }

    public async Task<bool> CreateNewWallPostReply(WallPostReply wallPostReply)
    {
        try
        {
            _db.WallPostReply.Add(wallPostReply);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[WallPostReplyRepository] WallPostReply CreateNewWallPostReply failed, error message: {E}", e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateWallPostReply(WallPostReply wallPostReply)
    {
        try
        {
            _db.WallPostReply.Update(wallPostReply);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "[WallPostReplyRepository] WallPostReply UpdateWallPostReply failed, error message: {E}", e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteWallPostReply(int wallPostReplyId)
    {
        try
        {
            var wallPostReply = await _db.WallPostReply.FindAsync(wallPostReplyId);
            if (wallPostReply == null)
            {
                _logger.LogError("[WallPostReplyRepository] WallPostReply DeleteWallPostReply failed, wallPostReply with id {Id} not found", wallPostReplyId);
                return false;
            }
            
            _db.WallPostReply.Remove(wallPostReply);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[WallPostReplyRepository] WallPostReply DeleteWallPostReply failed, error message: {E}", e.Message);
            return false;
        }
    }
}