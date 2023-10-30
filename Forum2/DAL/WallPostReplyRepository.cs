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

    public async Task<WallPostReply?> GetById(int id)
    {
        try
        {
            return await _db.WallPostReply.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[WallPostReplyRepository] WallPostReply GetAllByWallPostId failed, error message: {E}", e.Message);
            return null;
        }
    }

    public async Task<bool> Create(WallPostReply wallPostReply)
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

    public async Task<bool> Delete(int wallPostReplyId)
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

    public async Task<IEnumerable<WallPostReply>?> GetAllByCreator(string wallPostCreatorId)
    {
        List<WallPostReply> returnList = new List<WallPostReply>();
        try
        {
            var list = await _db.WallPostReply.ToListAsync();
            foreach (var wallReply in list)
            {
                if (wallReply.AuthorId == wallPostCreatorId)
                {
                    returnList.Add(wallReply);
                }
            }
            return returnList;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[WallPostReplyRepository] WallPostReply GetAllByWallPostId failed, error message: {E}", e.Message);
            return null;
        }
    }
}