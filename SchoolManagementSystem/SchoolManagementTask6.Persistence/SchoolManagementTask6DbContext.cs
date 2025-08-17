
using Microsoft.EntityFrameworkCore;
using SchoolManagementTask6.Domain.Courses;
using SchoolManagementTask6.Domain.Enrollments;
using SchoolManagementTask6.Domain.UserManager;
using System;

namespace SchoolManagementTask6.Persistence
{
   public class SchoolManagementTask6DbContext : DbContext
   {
        public SchoolManagementTask6DbContext(DbContextOptions<SchoolManagementTask6DbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }

}