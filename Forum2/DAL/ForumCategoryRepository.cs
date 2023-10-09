using Microsoft.EntityFrameworkCore;
using Forum2.Models;
using Forum2.ViewModels;

namespace Forum2.DAL;

public class ForumCategoryRepository : IForumCategoryRepository
{
    private readonly ForumDbContext _db;

    public ForumCategoryRepository(ForumDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ForumCategory>> GetAll()
    {
        return await _db.ForumCategory.ToListAsync();
    }
    
    public async Task<ForumCategory?> GetForumCategoryById(int id)
    {
        return await _db.ForumCategory.FindAsync(id);
    }

    public async Task CreateForumCategory(ForumCategory forumCategory)
    {
        _db.ForumCategory.Add(forumCategory);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateForumCategory(ForumCategory forumCategory)
    {
        _db.ForumCategory.Update(forumCategory);
        await _db.SaveChangesAsync();
    }
}