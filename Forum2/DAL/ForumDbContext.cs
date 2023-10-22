using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Forum2.Models;

public class ForumDbContext : DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {
        // Database.EnsureCreated();
    }
    public DbSet<ForumCategory> ForumCategory { get; set; }
    public DbSet<ForumThread> ForumThread { get; set; }
    public DbSet<ForumPost> ForumPost { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }


    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseLazyLoadingProxies();
    // }
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Account>()
    //         .HasOne(o => o.AccountRoles)
    //         .WithMany(c => c.Accounts)
    //         .HasForeignKey(o => o.RoleId);
    // }
}