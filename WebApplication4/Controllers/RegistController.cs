using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication4.Models;
using WebApplication4.ViewModels;


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
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Ошибки автоматически отобразятся валидацией
            }

            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Пользователь с таким именем уже существует");
                return View(model);
            }

            var newUser = new User
            {
                Username = model.Username,
                PasswordHash = model.Password, 
                Email = model.Email
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
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
