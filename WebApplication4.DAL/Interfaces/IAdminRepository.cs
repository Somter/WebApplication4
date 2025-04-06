using WebApplication4.DAL.Entities;

namespace WebApplication4.DAL.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin?> GetAdminByUsernameAsync(string username);
    }
}
