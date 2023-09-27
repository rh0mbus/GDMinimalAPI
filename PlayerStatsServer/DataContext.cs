global using Microsoft.EntityFrameworkCore;

namespace PlayerStatsServer
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=playerstatsdb;Trusted_Connection=true;TrustServerCertificate=true;");
        }

        public DbSet<Player> Players => Set<Player>();
    }
}
