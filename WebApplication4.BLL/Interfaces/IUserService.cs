using WebApplication4.BLL.DTO;

namespace WebApplication4.BLL.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(string username, string password, string email);
        Task<UserDTO?> AuthenticateUserAsync(string username, string password);
    }
}
