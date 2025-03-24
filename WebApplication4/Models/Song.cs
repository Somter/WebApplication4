using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string? Title { get; set; }

        [Required]
        public string? FilePath { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; } 
        public User? User { get; set; }

        [ForeignKey("Genre")]
        public int? GenreId { get; set; }
        public Genre? Genre { get; set; }
    }
}
