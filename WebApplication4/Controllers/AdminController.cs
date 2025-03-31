using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebApplication4.Models;
using WebApplication4.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Repository;

namespace WebApplication4.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminRepository _adminRepository;

        public AdminController(IUserRepository userRepository, IAdminRepository adminRepository)
        {
            _userRepository = userRepository;
            _adminRepository = adminRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
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
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var admin = await _adminRepository.GetAdminByUsernameAsync(model.Username);
                if (admin == null || admin.PasswordHash != model.Password)
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
        public async Task<IActionResult> EditUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
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

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _userRepository.GetUserByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Username = model.Username;
                user.Email = model.Email;
                user.IsActive = model.IsActive;

                await _userRepository.UpdateUserAsync(user);
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
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            try
            {
                await _userRepository.ToggleUserStatusAsync(id);  
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при изменении статуса пользователя: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при изменении статуса пользователя.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user != null)
                {
                    await _userRepository.DeleteUserAsync(user); 
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
