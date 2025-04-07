using Microsoft.AspNetCore.Mvc;
using WebApplication4.BLL.DTO;
using WebApplication4.BLL.Interfaces;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;

        public AdminController(IUserService userService, IAdminService adminService)
        {
            _userService = userService;
            _adminService = adminService;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync(); 

            var viewModel = users.Select(user => new UserDisplayViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive
            }).ToList();

            return View(viewModel);
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
                    return View(model);

                var admin = await _adminService.AuthenticateAdminAsync(model.Username, model.Password);
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
        public async Task<IActionResult> EditUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();

                var model = new EditUserViewModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    IsActive = user.IsActive
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке пользователя: {ex.Message}");
                TempData["ErrorMessage"] = "Ошибка при загрузке страницы редактирования.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var userDto = new UserDTO
                {
                    Id = model.Id,
                    Username = model.Username,
                    Email = model.Email,
                    IsActive = model.IsActive
                };

                await _userService.UpdateUserAsync(userDto);
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
                await _userService.ToggleUserStatusAsync(id);
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
                await _userService.DeleteUserAsync(id);
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
