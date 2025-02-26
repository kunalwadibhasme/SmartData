using App.Core.Interface;
using Domain.Entities;
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
        public DbSet<Users> users {  get; set; }
        public DbSet<Movies> movies { get; set; }
        public DbSet<Usertype> userstypes { get; set; }

        public IDbConnection GetConnection()
        {
            return this.Database.GetDbConnection();
        }
    }
}
