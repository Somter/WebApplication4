using Microsoft.EntityFrameworkCore;
using WebApplication4.DAL.Entities;

namespace WebApplication4.DAL.EF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<User> Users{ get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Song> Songs { get; set; }

    }
}
