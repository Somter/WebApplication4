using System.ComponentModel.DataAnnotations;
using WebApplication4.Models;

namespace WebApplication4.ViewModels
{
    public class MusicUploadViewModel
    {
        [Required]
        public string Title { get; set; } 

        [Required]
        public int GenreId { get; set; } 

        [Required]
        public IFormFile File { get; set; } 

        public List<Genre> Genres { get; set; } 
    }
}
