using Microsoft.EntityFrameworkCore;
using MyProject.Models;

namespace MyProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Ethnicity> Ethnicities { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<RewardReason> RewardReasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.ManagedEmployees)
                .HasForeignKey(e => e.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Admin)
                .WithMany(a => a.AdministeredEmployees)
                .HasForeignKey(e => e.AdminID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentID);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Ethnicity)
                .WithMany(et => et.Employees)
                .HasForeignKey(e => e.EthnicityID);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Region)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RegionID);

            modelBuilder.Entity<Reward>()
                .HasOne(r => r.Employee)
                .WithMany(e => e.Rewards)
                .HasForeignKey(r => r.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reward>()
                .HasOne(r => r.Issuer)
                .WithMany()
                .HasForeignKey(r => r.IssuerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Password>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.Passwords)
                .HasForeignKey(p => p.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
