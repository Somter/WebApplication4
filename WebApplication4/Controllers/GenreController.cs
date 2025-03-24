using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

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
            var genre = _context.Genre.ToList();
            return View(genre);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            var genre = _context.Genre.FirstOrDefault(g => g.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);  
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var genre = _context.Genre.FirstOrDefault(g => g.Id == id);

            if (genre != null)
            {
                _context.Genre.Remove(genre);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Genre model)
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

        [HttpPost]
        public IActionResult Create(Genre model)
        {
            if (_context.Genre.Any(g => g.Name == model.Name))
            {
                ViewBag.Error = "Такой жанр уже существует";
                return View(model);
            }

            _context.Genre.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
