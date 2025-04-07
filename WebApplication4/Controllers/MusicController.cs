using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.ViewModels;
using WebApplication4.BLL.Interfaces;
using WebApplication4.BLL.DTO;

namespace WebApplication4.Controllers
{
    public class MusicController : Controller
    {
        private readonly IMusicService _musicService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MusicController(IMusicService musicService, IWebHostEnvironment webHostEnvironment)
        {
            _musicService = musicService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var songs = await _musicService.GetAllSongsAsync();
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
        public async Task<IActionResult> Upload()
        {
            var model = new MusicUploadViewModel
            {
                Genres = await _musicService.GetAllGenresAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(MusicUploadViewModel model)
        {
            if (!ModelState.IsValid || model.File == null || model.File.Length == 0)
            {
                TempData["ErrorMessage"] = "Неверные данные или отсутствует файл.";
                model.Genres = await _musicService.GetAllGenresAsync();
                return View(model);
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Regist");

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Path.GetFileName(model.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            var songDto = new SongDTO
            {
                Title = model.Title,
                GenreId = model.GenreId ?? 0,
                FilePath = "/uploads/" + fileName,
                UserId = userId
            };

            await _musicService.AddSongAsync(songDto);
            TempData["SuccessMessage"] = "Песня успешно загружена!";
            return RedirectToAction("Upload");
        }

        public IActionResult Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return NotFound("Файл не найден.");

            var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var fullPath = Path.Combine(uploadsPath, fileName);

            if (!System.IO.File.Exists(fullPath)) return NotFound("Файл не найден.");

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "audio/mpeg", fileName);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSong(int id)
        {
            try
            {
                var song = await _musicService.GetSongByIdAsync(id);
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

                await _musicService.DeleteSongAsync(id);
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
        public async Task<IActionResult> Edit(int id)
        {
            var song = await _musicService.GetSongByIdAsync(id);
            if (song == null) return NotFound();

            var viewModel = new MusicUploadViewModel
            {
                Id = song.Id,
                Title = song.Title,
                GenreId = song.GenreId,
                Genres = await _musicService.GetAllGenresAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MusicUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _musicService.GetAllGenresAsync();
                return View(model);
            }

            var song = await _musicService.GetSongByIdAsync(model.Id);
            if (song == null) return NotFound();

            if (model.File != null && model.File.Length > 0)
            {
                var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, song.FilePath.TrimStart('/'));
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var fileName = Path.GetFileName(model.File.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                song.FilePath = "/uploads/" + fileName;
            }

            song.Title = model.Title;
            song.GenreId = model.GenreId ?? 0;

            await _musicService.UpdateSongAsync(song);
            TempData["SuccessMessage"] = "Песня успешно обновлена!";
            return RedirectToAction("Index");
        }
    }
}
