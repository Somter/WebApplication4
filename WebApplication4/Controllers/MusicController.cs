using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class MusicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MusicController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Upload()
        {

            ViewBag.Genres = _context.Genre.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(string Title, int GenreId, IFormFile File)
        {
            if (File == null || File.Length == 0)
            {
                TempData["ErrorMessage"] = "Пожалуйста, загрузите файл.";
                return RedirectToAction("Upload");
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Regist");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Regist");
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Path.GetFileName(File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }

            var newSong = new Song
            {
                Title = Title,
                GenreId = GenreId,
                FilePath = "/uploads/" + fileName,
                UserId = user.Id
            };

            _context.Songs.Add(newSong);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Песня успешно загружена!";
            return RedirectToAction("Upload");
        }


        public IActionResult Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound("Файл не найден.");
            }

            var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var fullPath = Path.Combine(uploadsPath, fileName);

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound("Файл не найден.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "audio/mpeg", fileName);
        }
    }
}
