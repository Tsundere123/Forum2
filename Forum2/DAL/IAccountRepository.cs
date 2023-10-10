using Forum2.Models;

namespace Forum2.DAL;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetAll();
    Task<Account?> GetAccountById(int id);
    Task Create(Account account);
    Task Update(Account account);
    Task<bool> Delete(int id);
}