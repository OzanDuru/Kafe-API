using Kafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Kafe.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // DbSet'lerinizi burada tanımlayın
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        

    }

}