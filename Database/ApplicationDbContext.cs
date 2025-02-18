
using DesktopApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DesktopApplication.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Corporation> Corporations { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<Branch>()
                .HasOne(b => b.Corporation)
                .WithMany(c => c.Branches)
                .HasForeignKey(b => b.CorporationId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Corporation)
                .WithMany()
                .HasForeignKey(u => u.CorporationId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Branch)
                .WithMany()
                .HasForeignKey(u => u.BranchId);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Corporation)
                .WithMany()
                .HasForeignKey(c => c.CorporationId);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Branch)
                .WithMany()
                .HasForeignKey(c => c.BranchId);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.CreatedByUserId);
        }
    }
}
