using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;

namespace StoreAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Stores)
                .WithOne()
                .HasForeignKey(s => s.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Store>()
                .HasMany(c => c.Products)
                .WithOne()
                .HasForeignKey(s => s.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}