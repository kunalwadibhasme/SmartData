using Microsoft.EntityFrameworkCore;
using ProductMicroservice.Entities;

namespace ProductMicroservice.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> products { get; set; }
    }
}
