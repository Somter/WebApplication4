using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using WebApplication4.DAL.EF;
using WebApplication4.BLL.Interfaces;
using WebApplication4.BLL.Services;
using WebApplication4.DAL.Interfaces;
using WebApplication4.DAL.Repositories;
using WebApplication4.DAL.Entities;
using WWebApplication4.DAL.Repositories;
using WebApplication4.Filters;
using WebApplication4;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

// Подключаем локализацию
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("ru"),
        new CultureInfo("en"),
        new CultureInfo("ja"),
        new CultureInfo("es"),
        new CultureInfo("ro")
    };

    options.DefaultRequestCulture = new RequestCulture("ru");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Підключаємо MVC + локалізація в DataAnnotations + Views
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(CultureFilter));
})
.AddViewLocalization()
.AddDataAnnotationsLocalization();

// Сервисы BLL
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMusicService, MusicService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// Сервисы DAL (репозитории)
builder.Services.AddScoped<IMusicRepository, MusicRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

// Контекст данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Настройка маршрутов
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

// Инициализация данных
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!context.Admin.Any())
    {
        context.Admin.Add(new Admin { Username = "admin1", PasswordHash = "12345" });
        context.SaveChanges();
    }
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!context.Genre.Any())
    {
        context.Genre.Add(new Genre { Name = "Рок" });
        context.SaveChanges();
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notification");  

app.Run();
