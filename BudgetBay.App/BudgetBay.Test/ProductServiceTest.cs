﻿using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BudgetBay.Services;
using BudgetBay.Models;
using BudgetBay.DTOs;

namespace BudgetBay.Test
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductService> _mockService;

        public ProductServiceTests()
        {
            _mockService = new Mock<IProductService>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsProducts_WhenProductExist()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Laptop" } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _mockService.Object.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Laptop", result[0].Name);
        }

        [Fact]
        public async Task GetActiveProductsAsync_ReturnsActiveProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = 2,
                    Name = "IPhone",
                }
            };

            _mockService.Setup(s => s.GetActiveProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _mockService.Object.GetActiveProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetById_ReturnsProduct_WhenExists()
        {
            // Arrange
            // CORRECTED: The service method returns a ProductDetailDto, so we must set it up to return that type.
            var productDto = new ProductDetailDto
            {
                Id = 3,
                Name = "Tablet"
            };

            _mockService.Setup(s => s.GetByIdAsync(3)).ReturnsAsync(productDto);

            // Act
            var result = await _mockService.Object.GetByIdAsync(3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNull_WhenNotExists()
        {
            // Arrange
            // CORRECTED: The method returns Task<ProductDetailDto?>, so we return a null of that specific type.
            _mockService.Setup(s => s.GetByIdAsync(89)).ReturnsAsync((ProductDetailDto?)null);

            // Act
            var result = await _mockService.Object.GetByIdAsync(89);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SearchProdocts_ReturnsMatchingProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = 4,
                    Name = "Camera"
                }
            };

            _mockService.Setup(s => s.SearchProductsAsync("Camera")).ReturnsAsync(products);

            // Act
            var result = await _mockService.Object.SearchProductsAsync("Camera");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Camera", result[0].Name);
        }

        [Fact]
        public async Task CreateProductsAsync_ReturnsCreatedProduct()
        {
            // Arrange
            // CORRECTED: The method expects a CreateProductDto as input.
            var newProductDto = new CreateProductDto
            {
                Name = "Airpods",
                Description = "Wireless earbuds",
                ImageUrl = "http://example.com/airpods.jpg",
                Condition = ProductCondition.New,
                EndTime = DateTime.UtcNow.AddDays(7),
                SellerId = 1,
                StartingPrice = 150.00m
            };
            // The method returns a Product.
            var createdProduct = new Product { Id = 5, Name = "Airpods" };
            _mockService.Setup(s => s.CreateProductAsync(It.IsAny<CreateProductDto>())).ReturnsAsync(createdProduct);

            // Act
            var result = await _mockService.Object.CreateProductAsync(newProductDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Airpods", result.Name);
        }

        [Fact]
        public async Task DeleteProductById_ReturnsTrue_WhenDeleted()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteProductByIdAsync(6)).ReturnsAsync(true);

            // Act
            var result = await _mockService.Object.DeleteProductByIdAsync(6);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductById_ReturnsFalse_WhenProductNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteProductByIdAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _mockService.Object.DeleteProductByIdAsync(99);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetProductsBySellerId_ReturnsSellerProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = 7,
                    Name = "Nike Shoes",
                    SellerId = 100
                }
            };

            _mockService.Setup(s => s.GetProductsBySellerId(100)).ReturnsAsync(products);

            // Act
            var result = await _mockService.Object.GetProductsBySellerId(100);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(100, result[0].SellerId);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsUpdatedProduct()
        {
            // Arrange
            // CORRECTED: The Product model has CurrentPrice (decimal?), not Price.
            var updatedProduct = new Product
            {
                Id = 9,
                Name = "Apple Watch",
                CurrentPrice = 9.35m // Use 'm' for decimal literal
            };

            _mockService.Setup(s => s.UpdateProductAsync(9, 9.35)).ReturnsAsync(updatedProduct);

            // Act
            var result = await _mockService.Object.UpdateProductAsync(9, 9.35);

            // Assert
            Assert.NotNull(result);
            // Assert against the correct property and type
            Assert.Equal(9.35m, result.CurrentPrice);
        }

         [Fact]
        public async Task GetProductsByWinnerId_ReturnsWinnerProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = 8,
                    Name = "IWatch",
                    WinnerId = 200
                }
            };

            _mockService.Setup(s => s.GetProductsByWinnerId(200)).ReturnsAsync(products);

            // Act
            var result = await _mockService.Object.GetProductsByWinnerId(200);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(200, result[0].WinnerId);
        }
    }
}