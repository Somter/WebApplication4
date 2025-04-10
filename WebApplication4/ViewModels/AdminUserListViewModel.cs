namespace WebApplication4.ViewModels
{
    public class AdminUserListViewModel
    {
        public List<UserDisplayViewModel> Users { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
