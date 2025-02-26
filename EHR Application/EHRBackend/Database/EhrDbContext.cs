using E_CommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceBackend.Database
{
    public class EhrDbContext : DbContext
    {
        public EhrDbContext(DbContextOptions<EhrDbContext> options): base(options) 
        { }

        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<PatientProvider> PatientProvider { get; set; }
        public DbSet<Soap> Soap { get; set; }
        public DbSet<Specialization> Specialization { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<StoreOtp> StoreOtp { get; set; }
        public DbSet<Usertype> Usertype { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<BloodGroup> BloodGroup { get; set; }


        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
