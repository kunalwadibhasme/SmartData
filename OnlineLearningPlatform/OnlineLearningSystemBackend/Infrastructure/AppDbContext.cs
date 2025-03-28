using Application.Interface;
using Domain.Enitities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Questions> questions { get; set; }
        public DbSet<Options> options { get; set; }
        public DbSet<Answer> answer { get; set; }

        public IDbConnection GetConnection()
        {
            return this.Database.GetDbConnection();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Course)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.courseId);

            // One-to-Many: Test has many Questions
            modelBuilder.Entity<Assignment>()
                .HasMany(t => t.Questions)
                .WithOne(q => q.Assignments)
                .HasForeignKey(q => q.AssignmentId);

            // One-to-Many: Question has many Options
            modelBuilder.Entity<Questions>()
                .HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId);

            // One-to-One: Question has one Answer
            modelBuilder.Entity<Questions>()
                .HasOne(q => q.Answer)
                .WithOne(a => a.Question)
                .HasForeignKey<Answer>(a => a.QuestionId);

            base.OnModelCreating(modelBuilder);
        }

    }

}
