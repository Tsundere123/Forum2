using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Forum2.Models;

public class InitDb
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        AccountDbContext accountDbContext = serviceScope.ServiceProvider.GetRequiredService<AccountDbContext>();
        ForumDbContext forumDbContext = serviceScope.ServiceProvider.GetRequiredService<ForumDbContext>();
        // context.Database.EnsureDeleted();
        accountDbContext.Database.EnsureCreated();
        forumDbContext.Database.EnsureCreated();

        //Account Roles
        if (!accountDbContext.Roles.Any())
        {
            accountDbContext.Roles.Add(new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(), 
                Name = "Administrator", 
                NormalizedName = "ADMINISTRATOR", 
                Color = "#FF0000", 
                IsFixed = true
            });
            accountDbContext.Roles.Add(new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Moderator",
                NormalizedName = "MODERATOR",
                Color = "#0000FF",
                IsFixed = true
            });
            accountDbContext.SaveChanges();
        }

        //User accounts
        if (!accountDbContext.Users.Any())
        {
            var admin = new ApplicationUser
            {
                Id = "9c46cebe-38d8-471c-ab0a-a1df156965d8",
                DisplayName = "Admin",
                Email = "admin@test.com",
                NormalizedEmail = "ADMIN@TEST.COM",
                UserName = "admin@test.com",
                NormalizedUserName = "ADMIN@TEST.COM",
                AvatarUrl = "546f26ec-9980-42e8-bf79-30fe3568998d.png",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var mod = new ApplicationUser
            {
                Id = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                DisplayName = "Mod",
                Email = "mod@test.com",
                NormalizedEmail = "MOD@TEST.COM",
                UserName = "mod@test.com",
                NormalizedUserName = "MOD@TEST.COM",
                AvatarUrl = "aa348e8e-fce6-422f-9c90-9c7dd92bc1fb.png",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var regular = new ApplicationUser
            {
                Id = "1f03d161-afc3-4507-9d6e-cdb55417b4ca",
                DisplayName = "User",
                Email = "user@test.com",
                NormalizedEmail = "USER@TEST.COM",
                UserName = "user@test.com",
                NormalizedUserName = "USER@TEST.COM",
                AvatarUrl = "default.png",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            
            var ceno = new ApplicationUser
            {
                Id = "dacf1d5f-3ae8-4b0a-bf7e-d00a633821a9",
                DisplayName = "Ceno",
                Email = "ceno@test.com",
                NormalizedEmail = "CENO@TEST.COM",
                UserName = "ceno@test.com",
                NormalizedUserName = "CENO@TEST.COM",
                AvatarUrl = "f55ec44f-88d8-4f25-b1da-e62b51797763.jpg",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var yes = new ApplicationUser
            {
                Id = "264cc5d0-3fb6-471c-a45e-296d46c71efa",
                DisplayName = "Yes",
                Email = "yes@no.com",
                NormalizedEmail = "YES@NO.COM",
                UserName = "yes@no.com",
                NormalizedUserName = "YES@NO.COM",
                AvatarUrl = "99d5e94a-cc07-4d5c-8205-166dbccdec96.jpg",
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            
            var pass = new PasswordHasher<ApplicationUser>();
            admin.PasswordHash = pass.HashPassword(admin, "Yes1234-");
            mod.PasswordHash = pass.HashPassword(mod, "Yes1234-");
            regular.PasswordHash = pass.HashPassword(regular, "Yes1234-");
            ceno.PasswordHash = pass.HashPassword(ceno, "Yes1234-");
            yes.PasswordHash = pass.HashPassword(yes, "Yes1234-");

            accountDbContext.Users.Add(admin);
            accountDbContext.Users.Add(mod);
            accountDbContext.Users.Add(regular);
            accountDbContext.Users.Add(ceno);
            accountDbContext.Users.Add(yes);

            accountDbContext.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = accountDbContext.Roles.FirstOrDefault(r => r.Name == "Administrator")?.Id ?? throw new InvalidOperationException(),
                UserId = admin.Id
            });
            
            accountDbContext.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = accountDbContext.Roles.FirstOrDefault(r => r.Name == "Moderator")?.Id ?? throw new InvalidOperationException(),
                UserId = mod.Id
            });
            
            accountDbContext.SaveChanges();
        }

        //Forum categories
        if (!forumDbContext.ForumCategory.Any())
        {
            var missiles = new ForumCategory()
            {
                ForumCategoryId = 1, 
                ForumCategoryName = "Missiles", 
                ForumCategoryDescription = "The missile knows where it is at all times"
            };

            var sheeps = new ForumCategory()
            {
                ForumCategoryId = 2, 
                ForumCategoryName = "Sheeps", 
                ForumCategoryDescription = "Poi"
            };
            
            var empty = new ForumCategory()
            {
                ForumCategoryId = 3, 
                ForumCategoryName = "Empty", 
                ForumCategoryDescription = "Nothing here"
            };
            
            forumDbContext.ForumCategory.Add(missiles);
            forumDbContext.ForumCategory.Add(sheeps);
            forumDbContext.ForumCategory.Add(empty);
            
            forumDbContext.SaveChanges();
        }

        //Forum Threads
        if (!forumDbContext.ForumThread.Any())
        {
            var first = new ForumThread()
            {
                ForumThreadId = 1,
                ForumThreadCreatorId = "9c46cebe-38d8-471c-ab0a-a1df156965d8",
                ForumThreadCreationTimeUnix = DateTime.Now,
                ForumCategoryId = 1, 
                ForumThreadTitle = "First thread"
            };
            
            var missileKnows = new ForumThread()
            {

                ForumThreadId = 2,
                ForumThreadCreatorId = "264cc5d0-3fb6-471c-a45e-296d46c71efa",
                ForumThreadCreationTimeUnix = DateTime.Now,
                ForumCategoryId = 1, 
                ForumThreadTitle = "The missile knows where it is at all times"
            };

            var yamato = new ForumThread()
            {
                ForumThreadId = 3,
                ForumThreadCreatorId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                ForumThreadCreationTimeUnix = DateTime.Now,
                ForumCategoryId = 2,
                ForumThreadTitle = "Yamato, the biggest battleship ever built"
            };
            
            var poi = new ForumThread()
            {
                ForumThreadId = 4,
                ForumThreadCreatorId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                ForumThreadCreationTimeUnix = DateTime.Now,
                ForumCategoryId = 2,
                ForumThreadTitle = "poipoipoi",
                ForumThreadIsSoftDeleted = true
            };
            
            var poidachi = new ForumThread()
            {
                ForumThreadId = 5,
                ForumThreadCreatorId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                ForumThreadCreationTimeUnix = DateTime.Now,
                ForumCategoryId = 2,
                ForumThreadTitle = "poipoipoi"
            };

            forumDbContext.ForumThread.Add(first);
            forumDbContext.ForumThread.Add(missileKnows);
            forumDbContext.ForumThread.Add(yamato);
            forumDbContext.ForumThread.Add(poi);
            forumDbContext.ForumThread.Add(poidachi);

            forumDbContext.SaveChanges();
        }

        //Forum Posts
        if (!forumDbContext.ForumPost.Any())
        {
            var post1 = new ForumPost()
            {
                ForumThreadId = 1,
                ForumPostId = 1,
                ForumPostContent = "This is in fact the first post",
                ForumPostCreatorId = "9c46cebe-38d8-471c-ab0a-a1df156965d8",
                ForumPostCreationTimeUnix = DateTime.Now,
            };
            var post2 = new ForumPost()
            {
                ForumThreadId = 1,
                ForumPostId = 2,
                ForumPostContent = "This is the second post",
                ForumPostCreatorId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                ForumPostCreationTimeUnix = DateTime.Now,
            };
            var post3 = new ForumPost()
            {
                ForumThreadId = 1,
                ForumPostId = 3,
                ForumPostContent = "This is the third post",
                ForumPostCreatorId = "1f03d161-afc3-4507-9d6e-cdb55417b4ca",
                ForumPostCreationTimeUnix = DateTime.Now,
                ForumPostIsSoftDeleted = true
            };
            
            //Thread 2
            var post4 = new ForumPost()
            {
                ForumThreadId = 2,
                ForumPostId = 4,
                ForumPostContent = "ThemissileknowswhereitisatalltimesItknowsthisbecauseitknowswhereitisntBysubtractingwhereitisfromwhereitisntorwhereitisntfromwhereitiswhicheverisgreateritobtainsadifferenceordeviationTheguidancesubsystemusesdeviationstogeneratecorrectivecommandstodrivethemissilefromapositionwhereitistoapositionwhereitisntandarrivingatapositionwhereitwasntitnowisConsequentlythepositionwhereitisisnowthepositionthatitwasntanditfollowsthatthepositionthatitwasisnowthepositionthatitisntIntheeventthatthepositionthatitisinisnotthepositionthatitwasntthesystemhasacquiredavariationthevariationbeingthedifferencebetweenwherethemissileisandwhereitwasntIfvariationisconsideredtobeasignificantfactorittoomaybecorrectedbytheGEAHoweverthemissilemustalsoknowwhereitwasThemissileguidancecomputerscenarioworksasfollowsBecauseavariationhasmodifiedsomeoftheinformationthemissilehasobtaineditisnotsurejustwhereitisHoweveritissurewhereitisntwithinreasonanditknowswhereitwasItnowsubtractswhereitshouldbefromwhereitwasntorviceversaandbydifferentiatingthisfromthealgebraicsumofwhereitshouldntbeandwhereitwasitisabletoobtainthedeviationanditsvariationwhichiscallederror",
                ForumPostCreatorId = "264cc5d0-3fb6-471c-a45e-296d46c71efa",
                ForumPostCreationTimeUnix = DateTime.Now,
            };
            var post5 = new ForumPost()
            {
                ForumThreadId = 2,
                ForumPostId = 5,
                ForumPostContent = "Really? A great wall of China??",
                ForumPostCreatorId = "9c46cebe-38d8-471c-ab0a-a1df156965d8",
                ForumPostCreationTimeUnix = DateTime.Now,
            };
            
            //Thread 3
            var post6 = new ForumPost()
            {
                ForumThreadId = 3,
                ForumPostId = 6,
                ForumPostContent = "![Yamato](https://upload.wikimedia.org/wikipedia/commons/e/e0/Yamato_sea_trials_2.jpg)",
                ForumPostCreatorId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                ForumPostCreationTimeUnix = DateTime.Now,
            };
            
            var post7 = new ForumPost()
            {
                ForumThreadId = 3,
                ForumPostId = 7,
                ForumPostContent = "46cm guns, they're big",
                ForumPostCreatorId = "1f03d161-afc3-4507-9d6e-cdb55417b4ca",
                ForumPostCreationTimeUnix = DateTime.Now,
            };
            
            //Thread 4
            var post8 = new ForumPost()
            {
                ForumThreadId = 4,
                ForumPostId = 8,
                ForumPostContent = "Poi~",
                ForumPostCreatorId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                ForumPostCreationTimeUnix = DateTime.Now,
                ForumPostIsSoftDeleted = true
            };
            //Thread 5
            var post9 = new ForumPost()
            {
                ForumThreadId = 5,
                ForumPostId = 9,
                ForumPostContent = "Poi~",
                ForumPostCreatorId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                ForumPostCreationTimeUnix = DateTime.Now,
            };
            
            forumDbContext.ForumPost.Add(post1);
            forumDbContext.ForumPost.Add(post2);
            forumDbContext.ForumPost.Add(post3);
            forumDbContext.ForumPost.Add(post4);
            forumDbContext.ForumPost.Add(post5);
            forumDbContext.ForumPost.Add(post6);
            forumDbContext.ForumPost.Add(post7);
            forumDbContext.ForumPost.Add(post8);
            forumDbContext.ForumPost.Add(post9);

            forumDbContext.SaveChanges();
        }

        //Wall posts
        if (!forumDbContext.WallPost.Any())
        {
            var wall1 = new WallPost()
            {
                Id = 1,
                AuthorId = "9c46cebe-38d8-471c-ab0a-a1df156965d8",
                ProfileId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                Content = "Hello there",
                CreatedAt = DateTime.Now
            };
            var wall2 = new WallPost()
            {
                Id = 2,
                AuthorId = "9c46cebe-38d8-471c-ab0a-a1df156965d8",
                ProfileId = "9c46cebe-38d8-471c-ab0a-a1df156965d8",
                Content = "This is my profile",
                CreatedAt = DateTime.Now
            };
            var wall3 = new WallPost()
            {
                Id = 3,
                AuthorId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                ProfileId = "9c46cebe-38d8-471c-ab0a-a1df156965d8",
                Content = "Very cool",
                CreatedAt = DateTime.Now
            };

            forumDbContext.WallPost.Add(wall1);
            forumDbContext.WallPost.Add(wall2);
            forumDbContext.WallPost.Add(wall3);

            forumDbContext.SaveChanges();
        }

        //Wall Posts Replies
        if (!forumDbContext.WallPostReply.Any())
        {
            var reply1 = new WallPostReply()
            {
                Id = 2,
                AuthorId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                WallPostId = 2,
                Content = "Yes it is :)",
                CreatedAt = DateTime.Now
            };
            var reply2 = new WallPostReply()
            {
                Id = 1,
                AuthorId = "ed8baa38-e5ee-4b93-8c50-0ecfe2726c2f",
                WallPostId = 2,
                Content = "Hi :)",
                CreatedAt = DateTime.Now
            };
            
            forumDbContext.WallPostReply.Add(reply1);
            forumDbContext.WallPostReply.Add(reply2);
            forumDbContext.SaveChanges();
        }
    }
}