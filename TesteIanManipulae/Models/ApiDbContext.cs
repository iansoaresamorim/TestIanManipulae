using Microsoft.EntityFrameworkCore;


namespace TesteIanManipulae.Models
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }

        public ApiDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
