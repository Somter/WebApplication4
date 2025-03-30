using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class GenreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GenreController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var genres = _context.Genre.ToList();
                return View(genres);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке жанров: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке списка жанров.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var genre = _context.Genre.FirstOrDefault(g => g.Id == id);
                if (genre == null)
                {
                    return NotFound();
                }

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

        public IActionResult Delete(int id)
        {
            try
            {
                var genre = _context.Genre.FirstOrDefault(g => g.Id == id);
                if (genre == null)
                {
                    return NotFound();
                }

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
                var genre = _context.Genre.FirstOrDefault(g => g.Id == id);
                if (genre != null)
                {
                    _context.Genre.Remove(genre);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении жанра: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при удалении жанра.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(GenreEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var genre = _context.Genre.FirstOrDefault(g => g.Id == model.Id);
                    if (genre == null)
                    {
                        return NotFound();
                    }

                    genre.Name = model.Name;
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при редактировании жанра: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при редактировании жанра.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Create(GenreCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.Genre.Any(g => g.Name == model.Name))
                    {
                        ViewBag.Error = "Такой жанр уже существует";
                        return View(model);
                    }

                    var newGenre = new Genre
                    {
                        Name = model.Name
                    };

                    _context.Genre.Add(newGenre);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при создании жанра: {ex.Message}");
                    TempData["ErrorMessage"] = "Ошибка при добавлении жанра.";
                    return RedirectToAction("Create");
                }
            }

            return View(model);
        }
    }
}
