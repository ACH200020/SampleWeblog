using BlogWeb.Infrastuctures;
using CoreLayer.Services.FileManagment;
using CoreLayer.Services.Post;
using CoreLayer.Services.User;
using CoreLayer.Services.UserRole;
using DataLayer.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<BlogContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IFileManager, FileManager>();
builder.Services.AddTransient<IUserRoleService, UserRoleService>();

builder.Services.AddJwtAuthentication(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["token"]?.ToString();
    if (string.IsNullOrWhiteSpace(token) == false)
    {
        context.Request.Headers.Append("Authorization", $"Bearer {token}");
    }
    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "Default",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapRazorPages();

app.Run();
