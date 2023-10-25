using Forum2.Models;

namespace Forum2.DAL;

public interface IWallPostReplyRepository
{
    Task<WallPostReply?> GetById(int id);
    
    Task<bool> Create(WallPostReply wallPostReply);
    Task<bool> Delete(int wallPostReplyId);
}