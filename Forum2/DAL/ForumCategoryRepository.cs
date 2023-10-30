using Microsoft.EntityFrameworkCore;
using Forum2.Models;

namespace Forum2.DAL;

public class ForumCategoryRepository : IForumCategoryRepository
{
    private readonly ForumDbContext _db;
    private readonly ILogger<ForumCategoryRepository> _logger;

    public ForumCategoryRepository(ForumDbContext db, ILogger<ForumCategoryRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<ForumCategory>?> GetAll()
    {
        try
        {
            return await _db.ForumCategory.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumCategoryRepository] ForumCategory GetAll failed, error message: {E}", e.Message);
            return null;
        }
    }
    
    public async Task<ForumCategory?> GetForumCategoryById(int id)
    {
        try
        {
            return await _db.ForumCategory.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumCategoryRepository] ForumCategory GetForumCategoryById failed, error message: {E}", e.Message);
            return null;
        }
    }

    public async Task<bool> CreateForumCategory(ForumCategory forumCategory)
    {
        try
        {
            _db.ForumCategory.Add(forumCategory);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumCategoryRepository] ForumCategory CreateForumCategory failed, error message: {E}", e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateForumCategory(ForumCategory forumCategory)
    {
        try
        {
            _db.ForumCategory.Update(forumCategory);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumCategoryRepository] ForumCategory UpdateForumCategory failed, error message: {E}", e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteForumCategory(ForumCategory forumCategory)
    {
        try
        {
            _db.ForumCategory.Remove(forumCategory);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[ForumCategoryRepository] ForumCategory DeleteForumCategory failed, error message: {E}", e.Message);
            return false;
        }
    }
}