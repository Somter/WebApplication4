using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using System.Linq;
using System.Diagnostics;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                return View(user); // ѕередаем пользовател€ в представление
            }

            return View(null); // ≈сли пользователь не найден, передаем null
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
