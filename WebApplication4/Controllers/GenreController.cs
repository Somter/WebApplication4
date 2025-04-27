using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApplication4.BLL.DTO;
using WebApplication4.BLL.Interfaces;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        IHubContext<NotificationHub> hubContext { get; }

        public GenreController(IGenreService genreService, IHubContext<NotificationHub> hub)
        {
            _genreService = genreService;
            hubContext = hub;
        }

        public IActionResult Index()
        {
            try
            {
                var genres = _genreService.GetAllGenres();
                return View(genres);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке жанров: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке списка жанров.";
                return RedirectToAction("Index");
            }
        }

        private async Task SendMessage(string message)
        {
            await hubContext.Clients.All.SendAsync("displayMessage", message);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(GenreCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if (_genreService.GenreExists(model.Name))
                {
                    ViewBag.Error = "Такой жанр уже существует";
                    return View(model);
                }

                var genreDto = new GenreDTO
                {
                    Name = model.Name
                };

                _genreService.AddGenre(genreDto);
                await SendMessage("Добавлен новый жанр: " + model.Name);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании жанра: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при добавлении жанра.";
                return RedirectToAction("Create");
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var genre = _genreService.GetGenreById(id);
                if (genre == null) return NotFound();

                var viewModel = new GenreEditViewModel
                {
                    Id = genre.Id,
                    Name = genre.Name
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при редактировании жанра: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке страницы редактирования.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GenreEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var genreDto = new GenreDTO
                {
                    Id = model.Id,
                    Name = model.Name
                };

                _genreService.UpdateGenre(genreDto);
                await SendMessage("Жанр: " + genreDto.Name + " отредактирован");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при редактировании жанра: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при редактировании жанра.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var genre = _genreService.GetGenreById(id);
                if (genre == null) return NotFound();

                return View(genre);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке страницы удаления: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке страницы удаления.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var gh = _genreService.GetGenreById(id);
                await SendMessage("Жанр " + gh.Name + " удалён");
                _genreService.DeleteGenre(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении жанра: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при удалении жанра.";
                return RedirectToAction("Index");
            }
        }
    }
}
