using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); // Это необходимо для работы сессий в памяти
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Таймаут сессии
    options.Cookie.HttpOnly = true; // Улучшенная безопасность
    options.Cookie.IsEssential = true; // Делаем куки сессии необходимыми
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseSession();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

using (var scope = app.Services.CreateScope()) 
{ 
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!context.Admin.Any())
    {
        context.Admin.Add(new Admin { Username = "admin1", PasswordHash = "12345" });
        context.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
