using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using WebApplication4.ViewModels;
using WebApplication4.BLL.Interfaces;

namespace WebApplication4.Controllers
{
    public class RegistController : Controller
    {
        private readonly IUserService _userService;

        public RegistController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registrationResult = await _userService.RegisterUserAsync(model.Username, model.Password, model.Email);

            if (!registrationResult)
            {
                ModelState.AddModelError("Username", "Пользователь с таким именем уже существует или пароль не соответствует требованиям.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Регистрация прошла успешно!";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.AuthenticateUserAsync(model.Username, model.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");

            return RedirectToAction("Login", "Regist");
        }


    }
}
