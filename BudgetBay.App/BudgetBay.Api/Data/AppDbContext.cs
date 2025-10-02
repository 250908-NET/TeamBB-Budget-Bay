using BudgetBay.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBay.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
             
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.ProductsForSale)
                .WithOne(p => p.Seller)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.WonProducts)
                .WithOne(p => p.Winner)
                .HasForeignKey(p => p.WinnerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bids)
                .WithOne(b => b.Bidder)
                .HasForeignKey(b => b.BidderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Bids)
                .WithOne(b => b.Product)
                .HasForeignKey(b => b.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(s => s.ProductsForSale)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne()
                .HasForeignKey<User>(u => u.AddressId);
        }
    }
}