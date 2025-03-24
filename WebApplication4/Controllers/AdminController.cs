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
            var user = _context.Users.ToList(); 
            return View(user);  
        }

        public IActionResult Login() => View();

        [HttpPost]  
        public IActionResult Login(Admin model) 
        {
            if (ModelState.IsValid) 
            {
                var admin = _context.Admin.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == model.PasswordHash);
                if (admin == null) 
                {
                    ViewBag.Error = "Неверное имя или пароль";
                    return View(model);
                }

                return RedirectToAction("Index", "Admin");  
            }

            return View(model); 
        }

        public IActionResult ActivateUser(int Id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == Id);

            if (user != null)
            {
                user.IsActive = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteUser(int Id)
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
        [HttpGet]
        public IActionResult EditUser(int Id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == Id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user); 
        }

        [HttpPost]
        public IActionResult EditUser(User model) 
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

    }
}
