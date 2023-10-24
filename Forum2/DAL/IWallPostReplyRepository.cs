using Forum2.Models;

namespace Forum2.DAL;

public interface IWallPostReplyRepository
{
    Task<IEnumerable<WallPostReply>?> GetAll();
    Task<IEnumerable<WallPostReply>?> GetAllByWallPostId(int wallPostId);
    
    Task<bool> CreateNewWallPostReply(WallPostReply wallPostReply);
    Task<bool> UpdateWallPostReply(WallPostReply wallPostReply);
    Task<bool> DeleteWallPostReply(int wallPostReplyId);
}