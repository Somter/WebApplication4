using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
