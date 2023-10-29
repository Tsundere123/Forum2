using Forum2.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum2;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AccountDbContext>(options => options.UseSqlite("AccountDbContextConnection"));
        services.AddDbContext<ForumDbContext>(options => options.UseSqlite("ForumDbContextConnection"));
    }
}