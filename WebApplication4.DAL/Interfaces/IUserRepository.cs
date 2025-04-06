using WebApplication4.DAL.Entities;

namespace WebApplication4.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int userId);
        Task<bool> UserExistsAsync(string username);
        Task AddUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task ToggleUserStatusAsync(int userId); 
    }
}
