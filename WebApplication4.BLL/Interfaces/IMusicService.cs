using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication4.BLL.DTO;

namespace WebApplication4.BLL.Interfaces
{
    using WebApplication4.BLL.DTO;

    public interface IMusicService
    {
        Task<List<SongDTO>> GetAllSongsAsync();
        Task<SongDTO?> GetSongByIdAsync(int id);
        Task<List<SongDTO>> GetSongsByGenreAsync(int genreId);
        Task<List<SongDTO>> GetSongsByUserAsync(int userId);
        Task<List<SongDTO>> SearchSongsByTitleAsync(string title);
        Task<bool> UpdateSongAsync(SongDTO songDto);
        Task<bool> DeleteSongAsync(int id);
        Task AddSongAsync(SongDTO songDto);
        Task<List<GenreDTO>> GetAllGenresAsync();
    }

}
