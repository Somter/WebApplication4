using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.ViewModels;
using Microsoft.AspNetCore.SignalR;
using WebApplication4.BLL.Interfaces;
using WebApplication4.BLL.DTO;
using WebApplication4.BLL.Services;
using WebApplication4.DAL.Entities;

namespace WebApplication4.Controllers
{
    public class MusicController : Controller
    {
        private readonly IMusicService _musicService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGenreService _genreService;
        IHubContext<NotificationHub> hubContext { get; }

        public MusicController(IMusicService musicService, IWebHostEnvironment webHostEnvironment, IGenreService genreService, IHubContext<NotificationHub> hub)
        {
            _musicService = musicService;
            _webHostEnvironment = webHostEnvironment;
            _genreService = genreService;
            hubContext = hub;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 3;

            try
            {
                var allSongs = await _musicService.GetAllSongsAsync();
                var totalSongs = allSongs.Count();
                var totalPages = (int)Math.Ceiling(totalSongs / (double)pageSize);

                var songsToShow = allSongs
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModel = new AdminSongsListViewModel
                {
                    Songs = songsToShow.Select(song => new AdminSongsViewModel
                    {
                        Id = song.Id,
                        Title = song.Title,
                        GenreName = song.Genre?.Name,
                        UserId = song.UserId,
                        FilePath = song.FilePath
                    }).ToList(),
                    CurrentPage = page,
                    TotalPages = totalPages
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при загрузке списка музыки.";
                Console.WriteLine($"Ошибка загрузки музыки: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task SendMessage(string message)
        {
            await hubContext.Clients.All.SendAsync("displayMessage", message);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            var genres = _genreService.GetAllGenres();
            var model = new MusicUploadViewModel
            {
                Genres = genres.Select(g => new GenreDTO { Id = g.Id, Name = g.Name }).ToList()
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
            await SendMessage("Добавлена новая музыка: " + model.Title);
            return RedirectToAction("Upload");
        }

        [HttpGet]
        public async Task<IActionResult> UploadAdmin()
        {
            var model = new MusicUploadViewModel
            {
                Genres = await _musicService.GetAllGenresAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAdmin(MusicUploadViewModel model)
        {
            if (!ModelState.IsValid || model.File == null || model.File.Length == 0)
            {
                TempData["ErrorMessage"] = "Неверные данные или отсутствует файл.";
                model.Genres = await _musicService.GetAllGenresAsync();
                return View(model);
            }

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
                UserId = null 
            };

            await _musicService.AddSongAsync(songDto);
            await SendMessage("Добавлена новая музыка: " + model.Title);
            return RedirectToAction("UploadAdmin");
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
                await SendMessage("Песня: " + song.Title + " удалена");
                await _musicService.DeleteSongAsync(id);
                TempData["SuccessMessage"] = "Песня успешно удалена!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ошибка при удалении песни.";
                Console.WriteLine($"Ошибка удаления песни: {ex.Message}");
            }
            await SendMessage("Удалена одна песня: ");
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
            await SendMessage("Песня: " + model.Title + " отредактирована");
            return RedirectToAction("Index");
        }
    }
}
