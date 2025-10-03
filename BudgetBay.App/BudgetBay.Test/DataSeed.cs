using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using BudgetBay.Models;
using BudgetBay.Data;

namespace BudgetBay.Test
{

    public class TestBase : IDisposable
    {
        protected AppDbContext Context { get; private set; }

        public TestBase()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            Context = new AppDbContext(options);
            Context.Database.EnsureCreated();
        }

        protected async Task SeedDataAsync()
        {
            var addresses = new List<Address>()
            {
                new Address { Id = 1, StreetNumber = "123", StreetName = "Main St", City = "Springfield", State = "IL", ZipCode = "62701" },
                new Address { Id = 2, StreetNumber = "456", StreetName = "Elm St", City = "Springfield", State = "IL", ZipCode = "62702" },
                new Address { Id = 3, StreetNumber = "789", StreetName = "Oak St", City = "Springfield", State = "IL", ZipCode = "62703" }
            };
            var bids = new List<Bid>()
            {
                new Bid { Id = 1, Amount = 50.00m, BidderId = 1, ProductId = 1, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new Bid { Id = 2, Amount = 75.00m, BidderId = 2, ProductId = 1, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                new Bid { Id = 3, Amount = 100.00m, BidderId = 3, ProductId = 2, CreatedAt = DateTime.UtcNow },
                new Bid { Id = 4, Amount = 150.00m, BidderId = 1, ProductId = 2, CreatedAt = DateTime.UtcNow }
            };
            var products = new List<Product>()
            {
                new Product { Id = 1, Name = "Laptop", Description = "A high-performance laptop", Condition = ProductCondition.New, StartTime = DateTime.UtcNow.AddDays(-5), EndTime = DateTime.UtcNow.AddDays(5), StartingPrice = 500.00m, CurrentPrice = 100.00m, SellerId = 1 },
                new Product { Id = 2, Name = "Smartphone", Description = "A latest model smartphone", Condition = ProductCondition.Used, StartTime = DateTime.UtcNow.AddDays(-3), EndTime = DateTime.UtcNow.AddDays(7), StartingPrice = 300.00m, CurrentPrice = 150.00m, SellerId = 2 },
                new Product { Id = 3, Name = "Headphones", Description = "Noise-cancelling headphones", Condition = ProductCondition.New, StartTime = DateTime.UtcNow.AddDays(-1), EndTime = DateTime.UtcNow.AddDays(9), StartingPrice = 100.00m, CurrentPrice = 100.00m, SellerId = 3 }

            };

            var users = new List<User>()
            {
                new User { Id = 1, Username = "johndoe", Email = "johndoe@example.com", PasswordHash = "hashedpassword", AddressId = 1 },
                new User { Id = 2, Username = "janedoe", Email = "janedoe@example.com", PasswordHash = "hashedpassword", AddressId = 2 },
                new User { Id = 3, Username = "alice", Email = "alice@example.com", PasswordHash = "hashedpassword", AddressId = 3 },
                new User { Id = 4, Username = "bob", Email = "bob@example.com", PasswordHash = "hashedpassword", AddressId = 1 }
            };

            Context.Users.AddRange(users);
            Context.Products.AddRange(products);
            Context.Bids.AddRange(bids);
            Context.Addresses.AddRange(addresses);
            await Context.SaveChangesAsync();
            
        }


        public void Dispose()
        {
            Context.Dispose();
        }


    }
}
