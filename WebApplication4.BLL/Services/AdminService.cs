using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApplication4.BLL.DTO;
using WebApplication4.BLL.Infrastructure;
using WebApplication4.BLL.Interfaces;
using WebApplication4.DAL.Interfaces;

namespace WebApplication4.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<AdminDTO?> AuthenticateAdminAsync(string username, string password)
        {
            var admin = await _adminRepository.GetAdminByUsernameAsync(username);
            if (admin == null || !PasswordHasher.Verify(password, admin.PasswordHash))
                return null;

            return new AdminDTO
            {
                Id = admin.Id,
                Username = admin.Username,
                PasswordHash = admin.PasswordHash
            };
        }
    }
}
