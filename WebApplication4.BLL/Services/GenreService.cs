using System;
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
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public List<GenreDTO> GetAllGenres()
        {
            var genres = _genreRepository.GetAllGenres();
            return genres.Select(g => new GenreDTO
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
        }

        public GenreDTO? GetGenreById(int id)
        {
            var genre = _genreRepository.GetGenreById(id);
            if (genre == null) return null;

            return new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public bool GenreExists(string name)
        {
            return _genreRepository.GenreExists(name);
        }

        public void AddGenre(GenreDTO genreDto)
        {
            var genre = new Genre
            {
                Name = genreDto.Name
            };

            _genreRepository.AddGenre(genre);
        }

        public void UpdateGenre(GenreDTO genreDto)
        {
            var genre = new Genre
            {
                Id = genreDto.Id,
                Name = genreDto.Name
            };

            _genreRepository.UpdateGenre(genre);
        }

        public void DeleteGenre(int id)
        {
            _genreRepository.DeleteGenre(id);
        }
    }
}
