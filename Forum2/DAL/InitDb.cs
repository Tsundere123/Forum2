namespace Forum2.Models;

public class InitDb
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        AccountDbContext context = serviceScope.ServiceProvider.GetRequiredService<AccountDbContext>();
        context.Database.EnsureDeleted();
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
    }
    
}