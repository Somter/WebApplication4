using System.ComponentModel.DataAnnotations;

namespace WebApplication4.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Имя пользователя должно содержать от 3 до 20 символов")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{6,}$", ErrorMessage = "Пароль должен содержать хотя бы одну заглавную букву, цифру и специальный символ")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
