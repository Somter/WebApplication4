using WebApplication4.BLL.DTO;
using WebApplication4.BLL.Interfaces;
using WebApplication4.DAL.Entities;
using WebApplication4.DAL.Interfaces;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
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

            if (!Validator.IsValidPassword(password))
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
    }

}
