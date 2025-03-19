using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }
}
