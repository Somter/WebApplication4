using WebApplication4.BLL.DTO;
using WebApplication4.BLL.Interfaces;
using WebApplication4.DAL.Entities;
using WebApplication4.DAL.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebApplication4.BLL.Infrastructure;

namespace WebApplication4.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterUserAsync(string username, string password, string email)
        {
            if (await _userRepository.UserExistsAsync(username))
                return false;

            var hashedPassword = PasswordHasher.Hash(password);

            var newUser = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Email = email
            };

            await _userRepository.AddUserAsync(newUser);
            return true;
        }

        public async Task<UserDTO?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !PasswordHasher.Verify(password, user.PasswordHash))
                return null;

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive
            };
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActive = user.IsActive
            };
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                IsActive = u.IsActive
            }).ToList();
        }

        public async Task UpdateUserAsync(UserDTO userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userDto.Id);
            if (user == null) return;

            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.IsActive = userDto.IsActive;

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task ToggleUserStatusAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return;

            user.IsActive = !user.IsActive; 

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user != null)
                await _userRepository.DeleteUserAsync(user);
        }
    }
}
