using System.ComponentModel.DataAnnotations;

namespace WebApplication4.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
