
using E_CommerceBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceBackend.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Usertable> Usertable { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Usertype> Usertype { get; set; }
        public DbSet<StoreOtp> StoreOtp { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cardtable> Cardtables { get; set; }
        public DbSet<Cartdetailtable> Cartdetailtables { get; set; }
        public DbSet<CartMastertable> CartMastertables { get; set; }
        public DbSet<SalesDetail> SalesDetails { get; set; }
        public DbSet<SalesMaster> SalesMasters { get; set; }



        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

       

    }
}

