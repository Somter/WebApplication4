using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApplication4.Models;
using WebApplication4.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    public class RegistController : Controller
    {
        private readonly IUserService _userService;

        public RegistController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register() => View(new RegisterViewModel());
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userExists = await _userService.RegisterUserAsync(model.Username, model.Password, model.Email);

            if (!userExists)
            {
                ModelState.AddModelError("Username", "Пользователь с таким именем уже существует");
                return View(model);
            }

            TempData["SuccessMessage"] = "Вы успешно зарегистрированы!";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _userService.AuthenticateUserAsync(model.Username, model.Password);
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
