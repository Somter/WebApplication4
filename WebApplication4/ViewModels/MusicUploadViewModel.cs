using System.ComponentModel.DataAnnotations;
using WebApplication4.BLL.DTO; 
using WebApplication4.Models;

namespace WebApplication4.ViewModels
{
    public class MusicUploadViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Выберите жанр")]
        public int? GenreId { get; set; }

        public IFormFile? File { get; set; }

        public List<GenreDTO>? Genres { get; set; }
    }
}
