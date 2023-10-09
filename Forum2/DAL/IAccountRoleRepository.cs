using Forum2.Models;

namespace Forum2.DAL;

public interface IAccountRoleRepository
{
    Task<IEnumerable<AccountRole>> GetAll();
}