using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApplication4.Models;
using WebApplication4.ViewModels;
using Microsoft.AspNetCore.Http;

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

        public IActionResult Login()
        {
            return View(new AdminLoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(AdminLoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var admin = _context.Admin.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == model.Password);
                if (admin == null)
                {
                    model.ErrorMessage = "Неверное имя или пароль";
                    return View(model);
                }

                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при входе администратора: {ex.Message}");
                model.ErrorMessage = "Ошибка при попытке входа.";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                var editUserViewModel = new EditUserViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    IsActive = user.IsActive
                };

                return View(editUserViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке страницы редактирования пользователя: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке страницы редактирования.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ActivateUser(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    user.IsActive = !user.IsActive;
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


        [HttpPost]
        public IActionResult EditUser(EditUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

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

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var songs = _context.Songs.Where(x => x.UserId == user.Id).ToList();
                    foreach (var song in songs)
                    {
                        song.UserId = null;  
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



    }
}
