using WebApplication4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
        public IActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);

                if (user == null || user.PasswordHash != model.PasswordHash)
                {
                    ViewBag.Error = "Неверный апроль или логин"; 
                    return View(); 
                }

                //HttpContext.Session.SetString("UserId", user.Id.ToString());
                //HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (model.PasswordHash != model.Email) 
                {
                    ViewBag.Error = "пароли не совпадают";
                    return View();
                }

                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ViewBag.Error = "Користувач вже існує";
                    return View();
                }

                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }
    }
}