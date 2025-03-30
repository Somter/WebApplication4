using System.ComponentModel.DataAnnotations;

namespace WebApplication4.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Имя пользователя должно содержать от 3 до 20 символов")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
