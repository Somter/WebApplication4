namespace WebApplication4.ViewModels
{
    public class AdminSongsListViewModel
    {
        public List<AdminSongsViewModel> Songs { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
