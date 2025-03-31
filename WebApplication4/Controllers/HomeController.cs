using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Repository;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMusicRepository _musicRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(IMusicRepository musicRepository, IUserRepository userRepository)
        {
            _musicRepository = musicRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                var songs = await _musicRepository.GetAllSongsWithGenresAsync();
                ViewBag.Songs = songs;

                User? user = null;
                if (userId != null)
                {
                    user = await _userRepository.GetUserByIdAsync(userId.Value);
                }

                return View(user);
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
