using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using WebApplication4.Repository;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public IActionResult Index()
        {
            try
            {
                var genres = _genreRepository.GetAllGenres();
                return View(genres);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке жанров: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке списка жанров.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(GenreCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                if (_genreRepository.GenreExists(model.Name))
                {
                    ViewBag.Error = "Такой жанр уже существует";
                    return View(model);
                }

                var newGenre = new Genre { Name = model.Name };
                _genreRepository.AddGenre(newGenre);

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
                var genre = _genreRepository.GetGenreById(id);
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
        public IActionResult Edit(GenreEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var genre = _genreRepository.GetGenreById(model.Id);
                if (genre == null) return NotFound();

                genre.Name = model.Name;
                _genreRepository.UpdateGenre(genre);

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
                var genre = _genreRepository.GetGenreById(id);
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
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _genreRepository.DeleteGenre(id);
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
