using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication4.DAL.Entities;

namespace WebApplication4.BLL.DTO
{
    public class SongDTO
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string? Title { get; set; }

        [Required]
        public string? FilePath { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        //public User? User { get; set; }

        [ForeignKey("Genre")]
        public int? GenreId { get; set; }
        public Genre? Genre { get; set; }
    }
}
