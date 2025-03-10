using Application.Interface;
using Domain.Enitities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> users { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<Enrollments> enrollments { get; set; }

        public IDbConnection GetConnection()
        {
            return this.Database.GetDbConnection();
        }

    }

}
