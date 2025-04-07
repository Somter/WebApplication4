using System.ComponentModel.DataAnnotations;

namespace WebApplication4.ViewModels
{
    public class UserDisplayViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string? Email { get; set; }

        public bool IsActive { get; set; }
    }
}
