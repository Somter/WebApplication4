using System.ComponentModel.DataAnnotations;

namespace WebApplication4.ViewModels
{
    public class AdminSongsViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? GenreName { get; set; }
        public int? UserId { get; set; }
        public string? FilePath { get; set; }
    }

}
