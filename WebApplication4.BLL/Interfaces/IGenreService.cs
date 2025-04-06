using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication4.BLL.DTO;

namespace WebApplication4.BLL.Interfaces
{
    public interface IGenreService
    {
        List<GenreDTO> GetAllGenres();
        GenreDTO? GetGenreById(int id);
        bool GenreExists(string name);
        void AddGenre(GenreDTO genreDto);
        void UpdateGenre(GenreDTO genreDto);
        void DeleteGenre(int id);
    }
}
