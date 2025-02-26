using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Customer.Microservice.Entities;

namespace CustomerMicroservice.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> customers { get; set; }
    }
}
