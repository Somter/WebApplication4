using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                var songs = await _context.Songs.Include(s => s.Genre).ToListAsync();
                ViewBag.Songs = songs;

                if (userId != null)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    return View(user);
                }

                return View(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке главной страницы: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке страницы.";
                return RedirectToAction("Error");
            }
        }

        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке страницы Privacy: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке страницы.";
                return RedirectToAction("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отображении страницы ошибки: {ex.Message}");
                return View(new ErrorViewModel { RequestId = "Неизвестная ошибка" });
            }
        }
    }
}
