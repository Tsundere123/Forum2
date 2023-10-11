using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Forum2.Models;

public class InitDb
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        AccountDbContext context = serviceScope.ServiceProvider.GetRequiredService<AccountDbContext>();
        // context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.AccountRoles.Any())
        {
            var accountRoles = new List<AccountRole>()
            {
                new AccountRole
                {
                  RoleName  = "Administrator",
                  RoleDescription = "Administrates the forum"
                },
                new AccountRole
                {
                    RoleName = "Moderator",
                    RoleDescription = "Moderates the forum"
                }
            };
            context.AddRange(accountRoles);
            context.SaveChanges();
        }
        if (!context.Accounts.Any())
        {
            var accounts = new List<Account>()
            {
                new Account
                {
                    AccountName = "Admin",
                    AccountPassword = "Yes",
                    AccountAvatar = "https://www.ikea.com/no/no/images/products/blahaj-toyleke-hai__0710175_pe727378_s5.jpg?f=s",
                    RoleId = 1
                        
                },
                new Account
                {
                    AccountName = "Mod",
                    AccountPassword = "No",
                    AccountAvatar = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/6e3a4157-8314-48a1-8b90-b03e1643ddee/d8qezl2-3f8544dc-4bb9-4a2f-95e8-77dffd19924a.png/v1/fill/w_750,h_750/kancolle_zuikaku_render_by_ryn_renders_d8qezl2-fullview.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7ImhlaWdodCI6Ijw9NzUwIiwicGF0aCI6IlwvZlwvNmUzYTQxNTctODMxNC00OGExLThiOTAtYjAzZTE2NDNkZGVlXC9kOHFlemwyLTNmODU0NGRjLTRiYjktNGEyZi05NWU4LTc3ZGZmZDE5OTI0YS5wbmciLCJ3aWR0aCI6Ijw9NzUwIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmltYWdlLm9wZXJhdGlvbnMiXX0.B6O1JlYtBqq7K527gy3XW7xPaHYSVQXbqljdJfU-G7I",
                    RoleId = 2
                }
            };
            context.AddRange(accounts);
            context.SaveChanges();
        }

        if (!context.Roles.Any())
        {
            context.Roles.Add(new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(), 
                Name = "Administrator", 
                NormalizedName = "ADMINISTRATOR", 
                Color = "#FF0000", 
                IsFixed = true
            });
            context.Roles.Add(new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Moderator",
                NormalizedName = "MODERATOR",
                Color = "#0000FF",
                IsFixed = true
            });
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            var admin = new ApplicationUser
            {
                DisplayName = "Administrator",
                Email = "admin@test.com",
                NormalizedEmail = "ADMIN@TEST.COM",
                UserName = "admin@test.com",
                NormalizedUserName = "ADMIN@TEST.COM",
                AvatarUrl = "https://art.pixilart.com/32cca92ae455838.png",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var mod = new ApplicationUser
            {
                DisplayName = "Moderator",
                Email = "mod@test.com",
                NormalizedEmail = "MOD@TEST.COM",
                UserName = "mod@test.com",
                NormalizedUserName = "MOD@TEST.COM",
                AvatarUrl = "https://art.pixilart.com/32cca92ae455838.png",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var regular = new ApplicationUser
            {
                DisplayName = "User",
                Email = "user@test.com",
                NormalizedEmail = "USER@TEST.COM",
                UserName = "user@test.com",
                NormalizedUserName = "USER@TEST.COM",
                AvatarUrl = "https://art.pixilart.com/32cca92ae455838.png",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            
            var pass = new PasswordHasher<ApplicationUser>();
            admin.PasswordHash = pass.HashPassword(admin, "Yes1234-");
            mod.PasswordHash = pass.HashPassword(mod, "Yes1234-");
            regular.PasswordHash = pass.HashPassword(regular, "Yes1234-");

            context.Users.Add(admin);
            context.Users.Add(mod);
            context.Users.Add(regular);

            context.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = context.Roles.FirstOrDefault(r => r.Name == "Administrator")?.Id ?? throw new InvalidOperationException(),
                UserId = admin.Id
            });
            
            context.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = context.Roles.FirstOrDefault(r => r.Name == "Moderator")?.Id ?? throw new InvalidOperationException(),
                UserId = mod.Id
            });
            
            context.SaveChanges();
        }
    }
    
}