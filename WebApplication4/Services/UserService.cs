//using System.Text.RegularExpressions;
//using System.Text;
//using WebApplication4.Models;
//using WebApplication4.Repository;
//using System.Security.Cryptography;

//namespace WebApplication4.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly IUserRepository _userRepository;

//        public UserService(IUserRepository userRepository)
//        {
//            _userRepository = userRepository;
//        }
//        public async Task<bool> RegisterUserAsync(string username, string password, string email)
//        {
//            if (await _userRepository.UserExistsAsync(username))
//            {
//                return false;
//            }

//            if (!IsValidPassword(password))
//            {
//                return false;
//            }

//            var hashedPassword = HashPassword(password);

//            var newUser = new User
//            {
//                Username = username,
//                PasswordHash = hashedPassword,
//                Email = email
//            };

//            await _userRepository.AddUserAsync(newUser);
//            return true;
//        }
//        public async Task<User?> AuthenticateUserAsync(string username, string password)
//        {
//            var user = await _userRepository.GetUserByUsernameAsync(username);
//            if (user == null || !VerifyPassword(password, user.PasswordHash))
//            {
//                return null;
//            }

//            return user;
//        }

//        private bool IsValidPassword(string password)
//        {
//            var passwordRegex = new Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{6,}$");
//            return passwordRegex.IsMatch(password);
//        }

//        private string HashPassword(string password)
//        {
//            using (SHA256 sha256 = SHA256.Create())
//            {
//                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//                return Convert.ToBase64String(bytes);
//            }
//        }
//        private bool VerifyPassword(string enteredPassword, string storedHash)
//        {
//            return HashPassword(enteredPassword) == storedHash;
//        }
//    }
//}
