using WebApplication4.Models;

namespace WebApplication4.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Genre> GetAllGenres() => _context.Genre.ToList();

        public Genre? GetGenreById(int id) => _context.Genre.FirstOrDefault(g => g.Id == id);

        public bool GenreExists(string name) => _context.Genre.Any(g => g.Name == name);

        public void AddGenre(Genre genre)
        {
            _context.Genre.Add(genre);
            _context.SaveChanges();
        }

        public void UpdateGenre(Genre genre)
        {
            _context.Genre.Update(genre);
            _context.SaveChanges();
        }

        public void DeleteGenre(int id)
        {
            var genre = _context.Genre.FirstOrDefault(g => g.Id == id);
            if (genre != null)
            {
                _context.Genre.Remove(genre);
                _context.SaveChanges();
            }
        }
    }
}
