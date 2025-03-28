using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var users = _context.Users.ToList();
                return View(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке пользователей: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке списка пользователей.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(Admin model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var admin = _context.Admin.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == model.PasswordHash);
                if (admin == null)
                {
                    ViewBag.Error = "Неверное имя или пароль";
                    return View(model);
                }

                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при входе администратора: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при попытке входа.";
                return RedirectToAction("Login");
            }
        }

        public IActionResult ActivateUser(int Id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == Id);
                if (user != null)
                {
                    user.IsActive = true;
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при активации пользователя: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при активации пользователя.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult DeleteUser(int Id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == Id);
                if (user != null)
                {
                    var songs = _context.Songs.Where(x => x.UserId == user.Id).ToList();
                    foreach (var item in songs)
                    {
                        item.UserId = null;
                    }

                    _context.Users.Remove(user);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении пользователя: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при удалении пользователя.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult EditUser(int Id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == Id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке страницы редактирования пользователя: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке страницы редактирования.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult EditUser(User model)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Username = model.Username;
                user.Email = model.Email;
                user.IsActive = model.IsActive;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при редактировании пользователя: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при редактировании пользователя.";
                return RedirectToAction("Index");
            }
        }
    }
}
