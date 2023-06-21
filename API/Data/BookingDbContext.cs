using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {

        }

        // Table
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }

        // Other Configuration or Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Constraints Unique
            modelBuilder.Entity<Employee>()
                        .HasIndex(e => new
                        {
                            e.Nik,
                            e.Email,
                            e.PhoneNumber
                        }).IsUnique();

            // University - Education (One to Many)
            modelBuilder.Entity<University>()
                        .HasMany(university => university.Educations)
                        .WithOne(education => education.University)
                        .HasForeignKey(education => education.UniversityGuid);

            /*modelBuilder.Entity<Education>()
                        .HasOne(e => e.University)
                        .WithMany(u => u.Educations)
                        .HasForeignKey(e => e.UniversityGuid)
                        .OnDelete(DeleteBehavior.Cascade);*/

            // Education - Employee (One to One)
            modelBuilder.Entity<Employee>()
                .HasOne(employee => employee.Education)
                .WithOne(education => education.Employee)
                .HasForeignKey<Education>(education => education.Guid);

            // Account - Employee (One to One)
            modelBuilder.Entity<Employee>()
                .HasOne(employee => employee.Account)
                .WithOne(account => account.Employee)
                .HasForeignKey<Account>(account => account.Guid);

            // Account - Account Role (One to Many)
            modelBuilder.Entity<Account>()
                .HasMany(account => account.AccountRole)
                .WithOne(roles => roles.Account)
                .HasForeignKey(roles => roles.AccountGuid);

            // Employee - Booking (One to Many)
            modelBuilder.Entity<Employee>()
                .HasMany(employee => employee.Bookings)
                .WithOne(booking => booking.Employee)
                .HasForeignKey(booking => booking.EmployeeGuid);

            // Room - Booking (One to Many)
            modelBuilder.Entity<Room>()
                .HasMany(room => room.Bookings)
                .WithOne(booking => booking.Room)
                .HasForeignKey(booking => booking.RoomGuid);

            // Role - Account Role (One to Many)
            modelBuilder.Entity<Role>()
                .HasMany(role => role.AccountRole)
                .WithOne(accountRole => accountRole.Role)
                .HasForeignKey(accountRole => accountRole.RoleGuid);
        }
    }
}
