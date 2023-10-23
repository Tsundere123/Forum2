﻿using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Forum2.Models;

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
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Account>()
    //         .HasOne(o => o.AccountRoles)
    //         .WithMany(c => c.Accounts)
    //         .HasForeignKey(o => o.RoleId);
    // }
}