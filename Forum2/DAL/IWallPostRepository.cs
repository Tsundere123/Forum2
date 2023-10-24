using Forum2.Models;

namespace Forum2.DAL;

public interface IWallPostRepository
{
    Task<IEnumerable<WallPost>?> GetAll();
    Task<IEnumerable<WallPost>?> GetAllByProfile(string id);
    Task<WallPost?> GetWallPostById(int id);
    
    Task<bool> CreateNewWallPost(WallPost wallPost);
    Task<bool> UpdateWallPost(WallPost wallPost);
    Task<bool> DeleteWallPost(int wallPostId);
}