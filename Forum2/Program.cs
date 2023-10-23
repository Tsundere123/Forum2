using Forum2.DAL;
using Microsoft.EntityFrameworkCore;
using Forum2.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AccountDbContextConnection") ??
                       throw new InvalidOperationException("Connection string 'AccountDbContextConnection' not found");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddDbContext<AccountDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:AccountDbContextConnection"]);
});
builder.Services.AddDbContext<ForumDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ForumDbContextConnection"]);
});

builder.Services.AddScoped<Forum2.DAL.IForumCategoryRepository, ForumCategoryRepository>();
builder.Services.AddScoped<Forum2.DAL.IForumThreadRepository, ForumThreadRepository>();
builder.Services.AddScoped<Forum2.DAL.IForumPostRepository, ForumPostRepository>();

builder.Services.AddDefaultIdentity<ApplicationUser>().AddRoles<ApplicationRole>().AddEntityFrameworkStores<AccountDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddSession();

// Logger
var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information().WriteTo
    .File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log");

loggerConfiguration.Filter.ByExcluding(e =>
    e.Properties.TryGetValue("SourceContext", out var value) && 
    e.Level == LogEventLevel.Information &&
    e.MessageTemplate.Text.Contains("Executed DbCommand"));

builder.Logging.AddSerilog(loggerConfiguration.CreateLogger());

var app = builder.Build();

// app.MapDefaultControllerRoute();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    InitDb.Seed(app);
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

//Backup of old

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{forumCategoryId?}");


app.Run();