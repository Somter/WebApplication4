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
            var songs = await _musicService.GetAllSongsAsync();

            if (!string.IsNullOrWhiteSpace(search))
                songs = songs.Where(s => s.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            if (genreId.HasValue && genreId.Value > 0)
                songs = songs.Where(s => s.GenreId == genreId.Value).ToList();

            songs = sortOrder switch
            {
                "Genre" => songs.OrderBy(s => s.Genre?.Name).ToList(),
                _ => songs.OrderBy(s => s.Title).ToList()
            };

            var genres = await _musicService.GetAllGenresAsync();

            var filterModel = new SongFilterViewModel
            {
                Search = search,
                SortOrder = sortOrder,
                GenreId = genreId,
                Songs = songs,
                Genres = genres,
                Users = new List<UserDisplayViewModel>()
            };

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var user = await _userService.GetUserByIdAsync(userId.Value);
                filterModel.Users.Add(new UserDisplayViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    IsActive = user.IsActive
                });
            }

            return View(filterModel);
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
