using Microsoft.EntityFrameworkCore;
using Forum2.Models;

namespace Forum2.DAL;

public class AccountRepository : IAccountRepository
{
    private readonly AccountDbContext _db;

    public AccountRepository(AccountDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAll()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task<Account?> GetAccountById(int id)
    {
        return await _db.Accounts.FindAsync(id);
    }

    public async Task Create(Account account)
    {
        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Account account)
    {
        _db.Accounts.Update(account);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var account = await _db.Accounts.FindAsync(id);
        if (account == null)
        {
            return false;
        }

        _db.Accounts.Remove(account);
        await _db.SaveChangesAsync();
        return true;
    }
}