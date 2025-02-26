using CustomerMicroService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerMicroService.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> customers { get; set; }
    }
}
