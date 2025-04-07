using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using System.Diagnostics;
using WebApplication4.BLL.Interfaces;
using WebApplication4.BLL.DTO;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMusicService _musicService;
        private readonly IUserService _userService;

        public HomeController(IMusicService musicService, IUserService userService)
        {
            _musicService = musicService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                var songs = await _musicService.GetAllSongsAsync();
                ViewBag.Songs = songs;

                UserDTO? userDTO = null;
                List<UserDisplayViewModel> userViewModels = new List<UserDisplayViewModel>();

                if (userId != null)
                {
                    // �������� ������������
                    userDTO = await _userService.GetUserByIdAsync(userId.Value);

                    // ����������� UserDTO � UserDisplayViewModel
                    userViewModels.Add(new UserDisplayViewModel
                    {
                        Id = userDTO.Id,
                        Username = userDTO.Username,
                        Email = userDTO.Email,
                        IsActive = userDTO.IsActive
                    });
                }

                // �������� � ������������� ������ �������������
                return View(userViewModels); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������ ��� �������� ������� ��������: {ex.Message}");
                TempData["ErrorMessage"] = "������ ��� �������� ��������.";
                return RedirectToAction("Error");
            }
        }



        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������ ��� �������� �������� Privacy: {ex.Message}");
                TempData["ErrorMessage"] = "������ ��� �������� ��������.";
                return RedirectToAction("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������ ��� ����������� �������� ������: {ex.Message}");
                return View(new ErrorViewModel { RequestId = "����������� ������" });
            }
        }
    }
}
