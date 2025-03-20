using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class RegistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Register(User model, string ConfirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (model.PasswordHash != ConfirmPassword)
                {
                    ViewBag.PasswordError = "Пароли не совпадают";
                    return View(model);
                }

                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ViewBag.Error = "Пользователь с таким именем уже существует";
                    return View(model);
                }

                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == model.PasswordHash);
                if (user == null)
                {
                    ViewBag.Error = "Неверный логин или пароль";
                    return View(model);
                }

                HttpContext.Session.SetInt32("UserId", user.Id); 

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "Regist"); 
        }

    }
}
