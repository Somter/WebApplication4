﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication4.BLL.DTO;
using WebApplication4.BLL.Interfaces;
using WebApplication4.DAL.Entities;
using WebApplication4.DAL.Interfaces;

namespace WebApplication4.BLL.Services
{
    public class MusicService : IMusicService
    {
        private readonly IMusicRepository _musicRepository;

        public MusicService(IMusicRepository musicRepository)
        {
            _musicRepository = musicRepository;
        }

        public async Task<List<SongDTO>> GetAllSongsAsync()
        {
            var songs = await _musicRepository.GetAllSongsWithGenresAsync();
            return songs.Select(MapToDTO).ToList();
        }

        public async Task<SongDTO?> GetSongByIdAsync(int id)
        {
            var song = await _musicRepository.GetSongByIdAsync(id);
            return song == null ? null : MapToDTO(song);
        }

        public async Task<List<SongDTO>> GetSongsByGenreAsync(int genreId)
        {
            var songs = await _musicRepository.GetSongsByGenreAsync(genreId);
            return songs.Select(MapToDTO).ToList();
        }

        public async Task<List<SongDTO>> GetSongsByUserAsync(int userId)
        {
            var songs = await _musicRepository.GetSongsByUserAsync(userId);
            return songs.Select(MapToDTO).ToList();
        }

        public async Task<List<SongDTO>> SearchSongsByTitleAsync(string title)
        {
            var songs = await _musicRepository.SearchSongsByTitleAsync(title);
            return songs.Select(MapToDTO).ToList();
        }

        public async Task<bool> UpdateSongAsync(SongDTO songDto)
        {
            var song = await _musicRepository.GetSongByIdAsync(songDto.Id);
            if (song == null) return false;

            song.Title = songDto.Title;
            song.GenreId = songDto.GenreId;
            song.FilePath = songDto.FilePath;
            song.UserId = songDto.UserId;

            await _musicRepository.UpdateSongAsync(song);
            return true;
        }

        public async Task<bool> DeleteSongAsync(int id)
        {
            var song = await _musicRepository.GetSongByIdAsync(id);
            if (song == null) return false;

            await _musicRepository.DeleteSongAsync(id);
            return true;
        }

        private SongDTO MapToDTO(Song song)
        {
            return new SongDTO
            {
                Id = song.Id,
                Title = song.Title,
                FilePath = song.FilePath,
                UserId = song.UserId,
                GenreId = song.GenreId
            };
        }
    }
}
