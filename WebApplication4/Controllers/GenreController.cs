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
            var ganre = _context.Genre.ToList();
            return View(ganre); 
        }

        public IActionResult Create() 
        {
            return View();
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
