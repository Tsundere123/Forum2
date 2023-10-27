using Forum2.DAL;
using Microsoft.EntityFrameworkCore;
using Forum2.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// DbContext
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

// Repositories
builder.Services.AddScoped<IForumCategoryRepository, ForumCategoryRepository>();
builder.Services.AddScoped<IForumThreadRepository, ForumThreadRepository>();
builder.Services.AddScoped<IForumPostRepository, ForumPostRepository>();

builder.Services.AddScoped<IWallPostRepository, WallPostRepository>();
builder.Services.AddScoped<IWallPostReplyRepository, WallPostReplyRepository>();

// Identity
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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    InitDb.Seed(app);
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.MapRazorPages();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}");

app.Run();