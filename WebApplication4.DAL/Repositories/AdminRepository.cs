using WebApplication4.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DAL.Interfaces;
using WebApplication4.DAL.EF;

namespace WebApplication4.DAL.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Admin?> GetAdminByUsernameAsync(string username)
        {
            return await _context.Admin.FirstOrDefaultAsync(a => a.Username == username);
        }
    }
}
