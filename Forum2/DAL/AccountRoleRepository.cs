using Forum2.Controllers;
using Forum2.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum2.DAL;

public class AccountRoleRepository : IAccountRoleRepository
{
    private readonly AccountDbContext _db;

    public AccountRoleRepository(AccountDbContext db)
    {
        _db = db;
    }
    
    public async Task<IEnumerable<AccountRole>> GetAll()
    {
        return await _db.AccountRoles.ToListAsync();
    }
}