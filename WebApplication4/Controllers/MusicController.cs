using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using WebApplication4.Repository;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class MusicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMusicRepository _musicRepository;

        public MusicController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IMusicRepository musicRepository)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _musicRepository = musicRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var songs = await _musicRepository.GetAllSongsWithGenresAsync();
                return View(songs);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при загрузке списка музыки.";
                Console.WriteLine($"Ошибка загрузки музыки: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public IActionResult Upload()
        {
            try
            {
                var model = new MusicUploadViewModel
                {
                    Genres = _context.Genre.ToList() 
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при загрузке страницы загрузки.";
                Console.WriteLine($"Ошибка при отображении страницы Upload: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upload(MusicUploadViewModel model)
        {
            try
            {
                if (model.File == null || model.File.Length == 0)
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

                var fileName = Path.GetFileName(model.File.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                var newSong = new Song
                {
                    Title = model.Title,
                    GenreId = model.GenreId,
                    FilePath = "/uploads/" + fileName,
                    UserId = user.Id
                };

                _context.Songs.Add(newSong);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Песня успешно загружена!";
                return RedirectToAction("Upload");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при загрузке песни. Попробуйте снова.";
                Console.WriteLine($"Ошибка загрузки песни: {ex.Message}");
                return RedirectToAction("Upload");
            }
        }

        public IActionResult Download(string fileName)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при скачивании файла {fileName}: {ex.Message}");
                return NotFound("Ошибка при скачивании файла.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSong(int id)
        {
            try
            {
                var song = await _musicRepository.GetSongByIdAsync(id);
                if (song == null)
                {
                    TempData["ErrorMessage"] = "Песня не найдена.";
                    return RedirectToAction("Index");
                }

                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, song.FilePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                await _musicRepository.DeleteSongAsync(id);
                TempData["SuccessMessage"] = "Песня успешно удалена!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при удалении песни.";
                Console.WriteLine($"Ошибка удаления песни: {ex.Message}");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UploadAdmin()
        {
            try
            {
                var model = new MusicUploadViewModel
                {
                    Genres = _context.Genre.ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при загрузке страницы загрузки для администратора.";
                Console.WriteLine($"Ошибка: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadAdmin(MusicUploadViewModel model)
        {
            try
            {
                if (model.File == null || model.File.Length == 0)
                {
                    TempData["ErrorMessage"] = "Пожалуйста, загрузите файл.";
                    return RedirectToAction("UploadAdmin");
                }

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Path.GetFileName(model.File.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                var newSong = new Song
                {
                    Title = model.Title,
                    GenreId = model.GenreId,
                    FilePath = "/uploads/" + fileName,
                    UserId = null 
                };

                _context.Songs.Add(newSong);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Песня успешно загружена администратором!";
                return RedirectToAction("UploadAdmin");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при загрузке песни. Попробуйте снова.";
                Console.WriteLine($"Ошибка: {ex.Message}");
                return RedirectToAction("UploadAdmin");
            }
        }

    }
}
