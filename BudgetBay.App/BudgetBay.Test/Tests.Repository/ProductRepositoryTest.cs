using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetBay.Data;
using BudgetBay.Models;
using BudgetBay.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace BudgetBay.Test.Repositories
{
    public class ProductRepositoryTest
    {
        private readonly Mock<AppDbContext> _mockAppDbContextMock;

        public ProductRepositoryTest()
        {
            _mockAppDbContextMock = new Mock<AppDbContext>();
        }

        private DbContextOptions<AppDbContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid()) // unique per test
                .Options;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            // Arrange
            var options = GetInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A" });
                context.Products.Add(new Product { Id = 2, Name = "Product B" });
                await context.SaveChangesAsync();
            }

            // Act
            List<Product> result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.GetAllAsync();
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "Product A");
            Assert.Contains(result, p => p.Name == "Product B");
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenNoProducts()
        {
            // Arrange
            var options = GetInMemoryOptions();
            // No products are added to the context
            // Act
            List<Product> result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.GetAllAsync();
            }
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProduct_WhenExists()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                // When testing methods with .Include(), related entities must also be added
                // to the in-memory database for the query to work correctly.
                var seller = new User { Id = 1, Username = "testseller", Email = "seller@test.com", PasswordHash = "hash" };
                context.Users.Add(seller);
                context.Products.Add(new Product { Id = 1, Name = "Product A", SellerId = 1 });
                await context.SaveChangesAsync();
            }
            // Act
            Product? result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.GetByIdAsync(1);
            }
            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Product A", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A" });
                await context.SaveChangesAsync();
            }
            // Act
            Product? result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.GetByIdAsync(99); // Non-existing ID
            }
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProductAsync_UpdatesPrice_WhenProductExists()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A", CurrentPrice = 100 });
                await context.SaveChangesAsync();
            }
            // Act
            Product? updatedProduct;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                updatedProduct = await repo.UpdateProductAsync(1, 150);
            }
            // Assert
            Assert.NotNull(updatedProduct);
            Assert.Equal(1, updatedProduct.Id);
            Assert.Equal(150, updatedProduct.CurrentPrice);
        }

        [Fact]
        public async Task UpdateProductAsync_ThrowsException_WhenProductNotFound()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A", CurrentPrice = 100 });
                await context.SaveChangesAsync();
            }
            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                {
                    await repo.UpdateProductAsync(99, 150); // Non-existing ID
                });
            }
        }
        [Fact]
        public async Task DeleteProductByIdAsync_DeletesProduct_WhenExists()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A" });
                await context.SaveChangesAsync();
            }
            // Act
            bool deleteResult;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                deleteResult = await repo.DeleteProductByIdAsync(1);
            }
            // Assert
            Assert.True(deleteResult);
            using (var context = new AppDbContext(options))
            {
                var product = await context.Products.FindAsync(1);
                Assert.Null(product); // Product should be deleted
            }
        }
        [Fact]
        public async Task DeleteProductByIdAsync_ReturnsFalse_WhenNotExists()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A" });
                await context.SaveChangesAsync();
            }
            // Act
            bool deleteResult;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                deleteResult = await repo.DeleteProductByIdAsync(99); // Non-existing ID
            }
            // Assert
            Assert.False(deleteResult);

            using (var context = new AppDbContext(options))
            {
                var product = await context.Products.FindAsync(1);
                Assert.NotNull(product); // Existing product should still be there
            }
        }

        [Fact]
        public async Task UpdateProductAsync_UpdatesEntireProduct_WhenProductExists()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A", CurrentPrice = 100 });
                await context.SaveChangesAsync();
            }
            // Act
            Product? updatedProduct;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                var productToUpdate = new Product { Id = 1, Name = "Updated Product A", CurrentPrice = 200 };
                updatedProduct = await repo.UpdateProductAsync(productToUpdate);
            }
            // Assert
            Assert.NotNull(updatedProduct);
            Assert.Equal(1, updatedProduct.Id);
            Assert.Equal("Updated Product A", updatedProduct.Name);
            Assert.Equal(200, updatedProduct.CurrentPrice);
        }

        [Fact]
        public async Task UpdateProductAsync_ThrowsException_WhenProductToUpdateNotFound()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A", CurrentPrice = 100 });
                await context.SaveChangesAsync();
            }
            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                var productToUpdate = new Product { Id = 99, Name = "Non-existing Product", CurrentPrice = 200 }; // Non-existing ID
                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
                {
                    await repo.UpdateProductAsync(productToUpdate);
                });
            }
        }

        [Fact]
        public async Task SearchProductsAsync_ReturnsMatchingProducts()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Apple iPhone", Description = "Latest model" });
                context.Products.Add(new Product { Id = 2, Name = "Samsung Galaxy", Description = "New release" });
                context.Products.Add(new Product { Id = 3, Name = "Google Pixel", Description = "Great camera" });
                await context.SaveChangesAsync();
            }
            // Act
            List<Product> result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.SearchProductsAsync("Galaxy");
            }
            // Assert
            Assert.Single(result);
            Assert.Equal("Samsung Galaxy", result[0].Name);


        }

        [Fact]
        public async Task SearchProductsAsync_ReturnsEmptyList_WhenNoMatches()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Apple iPhone", Description = "Latest model" });
                context.Products.Add(new Product { Id = 2, Name = "Samsung Galaxy", Description = "New release" });
                await context.SaveChangesAsync();
            }
            // Act
            List<Product> result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.SearchProductsAsync("Nokia");
            }
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductBySellerIdAsync_ReturnsProducts_WhenExists()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A", SellerId = 1 });
                context.Products.Add(new Product { Id = 2, Name = "Product B", SellerId = 2 });
                context.Products.Add(new Product { Id = 3, Name = "Product C", SellerId = 1 });
                await context.SaveChangesAsync();
            }
            // Act
            List<Product> result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.GetProductsBySellerId(1);
            }
            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Equal(1, p.SellerId));
        }
        [Fact]
        public async Task GetProductBySellerIdAsync_ReturnsEmptyList_WhenNoProducts()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A", SellerId = 1 });
                context.Products.Add(new Product { Id = 2, Name = "Product B", SellerId = 2 });
                await context.SaveChangesAsync();
            }
            // Act
            List<Product> result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.GetProductsBySellerId(99); // Non-existing SellerId
            }
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductByWinnerIdAsync_ReturnsProducts_WhenExists()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A", WinnerId = 1 });
                context.Products.Add(new Product { Id = 2, Name = "Product B", WinnerId = 2 });
                context.Products.Add(new Product { Id = 3, Name = "Product C", WinnerId = 1 });
                await context.SaveChangesAsync();
            }
            // Act
            List<Product> result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.GetProductsByWinnerId(1);
            }
            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Equal(1, p.WinnerId));
        }

        [Fact]
        public async Task GetProductByWinnerIdAsync_ReturnsEmptyList_WhenNoProducts()
        {
            // Arrange
            var options = GetInMemoryOptions();
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A", WinnerId = 1 });
                context.Products.Add(new Product { Id = 2, Name = "Product B", WinnerId = 2 });
                await context.SaveChangesAsync();
            }
            // Act
            List<Product> result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.GetProductsByWinnerId(99); // Non-existing WinnerId
            }
            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetActiveProductsAsync_ReturnsOnlyActiveProducts()
        {
            // Arrange
            var options = GetInMemoryOptions();
            var now = DateTime.UtcNow;
            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product { Id = 1, Name = "Active Product", EndTime = now.AddHours(1) });
                context.Products.Add(new Product { Id = 2, Name = "Expired Product", EndTime = now.AddHours(-1) });
                await context.SaveChangesAsync();
            }
            // Act
            List<Product> result;
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                result = await repo.GetActiveProductsAsync();
            }
            // Assert
            Assert.Single(result);
            Assert.Equal("Active Product", result[0].Name);
        }

        [Fact]
        public async Task CreateProductAsync_AddsProductSuccessfully()
        {
            // Arrange
            var options = GetInMemoryOptions();
            var newProduct = new Product { Id = 1, Name = "New Product", Description = "New product description" };
            // Act
            using (var context = new AppDbContext(options))
            {
                var repo = new ProductRepository(context);
                await repo.CreateProductAsync(newProduct);
            }
            // Assert
            using (var context = new AppDbContext(options))
            {
                var product = await context.Products.FindAsync(1);
                Assert.NotNull(product);
                Assert.Equal("New Product", product.Name);
                Assert.Equal("New product description", product.Description);
            }
        }

    }
}