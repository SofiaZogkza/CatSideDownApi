using Microsoft.EntityFrameworkCore;

namespace CatSideDownApi.Contracts
{
    public class CatSideDownDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
    }
}
