using WebApplication4.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Repository
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
