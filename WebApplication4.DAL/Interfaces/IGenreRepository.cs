using WebApplication4.DAL.Entities;

namespace WebApplication4.DAL.Interfaces
{
    public interface IGenreRepository
    {
        List<Genre> GetAllGenres();
        Genre? GetGenreById(int id);
        bool GenreExists(string name);
        void AddGenre(Genre genre);
        void UpdateGenre(Genre genre);
        void DeleteGenre(int id);
    }

}
