using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using System.Diagnostics;
using WebApplication4.BLL.Interfaces;
using WebApplication4.BLL.DTO;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMusicService _musicService;
        private readonly IUserService _userService;

        public HomeController(IMusicService musicService, IUserService userService)
        {
            _musicService = musicService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(string search, string sortOrder, int? genreId)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                var songs = await _musicService.GetAllSongsAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    songs = songs.Where(s => s.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (genreId.HasValue && genreId.Value > 0)
                {
                    songs = songs.Where(s => s.GenreId == genreId.Value).ToList();
                }

                switch (sortOrder)
                {
                    case "Genre":
                        songs = songs.OrderBy(s => s.Genre?.Name).ToList();
                        break;
                    case "Title":
                    default:
                        songs = songs.OrderBy(s => s.Title).ToList();
                        break;
                }

                ViewBag.Songs = songs;

                var genres = await _musicService.GetAllGenresAsync();
                ViewBag.Genres = genres;

                UserDTO? userDTO = null;
                List<UserDisplayViewModel> userViewModels = new List<UserDisplayViewModel>();

                if (userId != null)
                {
                    userDTO = await _userService.GetUserByIdAsync(userId.Value);

                    userViewModels.Add(new UserDisplayViewModel
                    {
                        Id = userDTO.Id,
                        Username = userDTO.Username,
                        Email = userDTO.Email,
                        IsActive = userDTO.IsActive
                    });
                }

                return View(userViewModels);
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
