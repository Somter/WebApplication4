using WebApplication4.BLL.DTO;

namespace WebApplication4.ViewModels
{
    public class SongFilterViewModel
    {
        public string? Search { get; set; }
        public string? SortOrder { get; set; }
        public int? GenreId { get; set; }
        public List<GenreDTO>? Genres { get; set; }
        public List<SongDTO>? Songs { get; set; }
        public List<UserDisplayViewModel>? Users { get; set; }
    }

}
