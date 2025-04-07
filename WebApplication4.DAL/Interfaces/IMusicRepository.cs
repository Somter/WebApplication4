using WebApplication4.DAL.Entities;

namespace WebApplication4.DAL.Interfaces
{
    public interface IMusicRepository
    {
        Task<List<Song>> GetAllSongsWithGenresAsync();
        Task<List<Song>> GetSongsByGenreAsync(int genreId);
        Task<List<Song>> GetSongsByUserAsync(int userId);
        Task<List<Song>> SearchSongsByTitleAsync(string title);
        Task UpdateSongAsync(Song song);
        Task DeleteSongAsync(int songId);
        Task<List<Genre>> GetAllGenresAsync();
        Task<Song> GetSongByIdAsync(int songId);
        Task AddSongAsync(Song song);
    }

}
