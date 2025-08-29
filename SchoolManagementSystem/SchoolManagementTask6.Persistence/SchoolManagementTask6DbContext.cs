
using Microsoft.EntityFrameworkCore;
using SchoolManagementTask6.Domain.Courses;
using SchoolManagementTask6.Domain.Departments;
using SchoolManagementTask6.Domain.Lecturers;
using SchoolManagementTask6.Domain.Students;
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
        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Course → Lecturer (disable cascade delete)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Lecturers)
                .WithMany() // not defining navigation back for simplicity
                .HasForeignKey(c => c.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);

            //  Course → Department
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Departments)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student → Department
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Lecturer → Department
            modelBuilder.Entity<Lecturer>()
                .HasOne(l => l.Department)
                .WithMany(d => d.Lecturers)
                .HasForeignKey(l => l.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //  Student → User
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //  Lecturer → User
            modelBuilder.Entity<Lecturer>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }

}