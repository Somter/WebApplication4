using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApplication4.Models;
using WebApplication4.ViewModels;
using Microsoft.AspNetCore.Http;

namespace WebApplication4.Controllers
{
    public class RegistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register() => View(new RegisterViewModel());
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Пользователь с таким именем уже существует");
                    return View(model);
                }

                var newUser = new User
                {
                    Username = model.Username,
                    PasswordHash = model.Password, // ВАЖНО: нужно хешировать пароль
                    Email = model.Email
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка при регистрации. Попробуйте еще раз.");
                Console.WriteLine($"Ошибка регистрации: {ex.Message}");
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == model.Password);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                    return View(model);
                }

                HttpContext.Session.SetInt32("UserId", user.Id);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка при входе. Попробуйте еще раз.");
                Console.WriteLine($"Ошибка входа: {ex.Message}");
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Regist");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выхода: {ex.Message}");
                return RedirectToAction("Login", "Regist");
            }
        }
    }
}
