using Forum2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum2.DAL;

public class AccountDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
        // Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}