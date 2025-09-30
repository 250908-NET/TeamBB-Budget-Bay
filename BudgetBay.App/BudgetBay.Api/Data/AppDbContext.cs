using BudgetBay.Api.Models;
using BudgetBay.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBay.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}