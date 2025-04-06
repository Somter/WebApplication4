using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication4.BLL.DTO;

namespace WebApplication4.BLL.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDTO?> AuthenticateAdminAsync(string username, string password);
    }
}
