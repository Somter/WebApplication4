using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
