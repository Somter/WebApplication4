using WebApplication4.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DAL.Interfaces;
using WebApplication4.DAL.EF;

namespace WebApplication4.DAL.Repositories
{
    public class MusicRepository : IMusicRepository
    {
        private readonly ApplicationDbContext _context;

        public MusicRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Song>> GetAllSongsWithGenresAsync()
        {
            return await _context.Songs.Include(s => s.Genre).ToListAsync();
        }

        public async Task<List<Song>> GetSongsByGenreAsync(int genreId)
        {
            return await _context.Songs
                                 .Where(s => s.GenreId == genreId)
                                 .Include(s => s.Genre)
                                 .ToListAsync();
        }

        public async Task<List<Song>> GetSongsByUserAsync(int userId)
        {
            return await _context.Songs
                                 .Where(s => s.UserId == userId)
                                 .Include(s => s.Genre)
                                 .ToListAsync();
        }

        public async Task<List<Song>> SearchSongsByTitleAsync(string title)
        {
            return await _context.Songs
                                 .Where(s => s.Title.Contains(title))
                                 .Include(s => s.Genre)
                                 .ToListAsync();
        }

        public async Task UpdateSongAsync(Song song)
        {
            _context.Songs.Update(song);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSongAsync(int songId)
        {
            var song = await _context.Songs.FindAsync(songId);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _context.Genre.ToListAsync();
        }

        public async Task<Song> GetSongByIdAsync(int songId)
        {
            return await _context.Songs
                                 .Include(s => s.Genre)
                                 .FirstOrDefaultAsync(s => s.Id == songId);
        }
    }

}
