using System.ComponentModel.DataAnnotations;

namespace WebApplication4.ViewModels
{
    public class GenreEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название жанра обязательно")]
        [StringLength(100, ErrorMessage = "Название жанра не может быть длиннее 100 символов")]
        public string Name { get; set; } 
    }
}
