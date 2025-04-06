using System.ComponentModel.DataAnnotations;

namespace WebApplication4.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
