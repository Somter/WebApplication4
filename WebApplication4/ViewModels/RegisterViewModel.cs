using System.ComponentModel.DataAnnotations;
using WebApplication4.Resources;

namespace WebApplication4.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "UserrnameRequired", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(20, MinimumLength = 3, ErrorMessageResourceName = "UsernameLength", ErrorMessageResourceType = typeof(Resource))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{6,}$", ErrorMessageResourceName = "PasswordStrength_", ErrorMessageResourceType = typeof(Resource))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired_", ErrorMessageResourceType = typeof(Resource))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmPasswordMismatch", ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
