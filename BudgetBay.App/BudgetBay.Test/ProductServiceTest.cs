﻿using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BudgetBay.Services;
using BudgetBay.Models;
using BudgetBay.Repositories;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace BudgetBay.Test
{
    public class ProductServiceTests
    {
        private readonly ProductService _service;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<ProductService>> _mockLogger;

        public ProductServiceTests()
        {
            _mockProductRepo = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _mapper = new Mock<IMapper>();

            _service = new ProductService(
                _mockLogger.Object,
                _mockProductRepo.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsProducts_WhenProductsExist()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Laptop" } };
            _mockProductRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _service.GetAllAsync(); // call the real service

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

            _mockProductRepo.Setup(s => s.GetActiveProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _service.GetActiveProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("IPhone", result[0].Name);
        }

        [Fact]
        public async Task GetById_ReturnsProduct_WhenExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 3,
                Name = "Tablet"
            };

            _mockProductRepo.Setup(s => s.GetByIdAsync(3)).ReturnsAsync(product);

            // Act
            var result = await _service.GetByIdAsync(3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNull_WhenNotExists()
        {
            // Arrange
            _mockProductRepo.Setup(s => s.GetByIdAsync(89)).ReturnsAsync((Product?)null);

            // Act
            var result = await _service.GetByIdAsync(89);

            // Arrange
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

            _mockProductRepo.Setup(s => s.SearchProductsAsync("Camera")).ReturnsAsync(products);

            // Act
            var result = await _service.SearchProductsAsync("Camera");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Camera", result[0].Name);
        }

        /*[Fact]
        public async Task CreateProductsAsync_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = new Product { Id = 5, Name = "Airpods" };
            _mockService.Setup(s => s.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(newProduct);

            // Act
            var result = await _mockService.Object.CreateProductAsync(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Airpods", result.Name);
        }*/

        [Fact]
        public async Task DeleteProductById_ReturnsTrue_WhenDeleted()
        {
            // Arrange
            _mockProductRepo.Setup(s => s.DeleteProductByIdAsync(6)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteProductByIdAsync(6);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductById_ReturnsFalse_WhenDeleted()
        {
            // Arrange
            _mockProductRepo.Setup(s => s.DeleteProductByIdAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteProductByIdAsync(99);

            // Arrange
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

            _mockProductRepo.Setup(s => s.GetProductsBySellerId(100)).ReturnsAsync(products);

            // Act
            var result = await _service.GetProductsBySellerId(100);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(100, result[0].SellerId);
        }

        /*[Fact]
        public async Task UpdateProduct_ReturnsUpdatedProduct()
        {
            // Arrange
            var updatedProduct = new Product
            {
                Id = 9,
                Name = "Apple Watch",
                Price = 9.35
            };

            _mockService.Setup(s => s.UpdateProductAsync(9, 9.35)).ReturnsAsync(updatedProduct);

            // Act
            var result = await _mockService.Object.UpdateProductAsync(9, 9.35);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(9.35, result.Price);
        }*/

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

            _mockProductRepo.Setup(s => s.GetProductsByWinnerId(200)).ReturnsAsync(products);

            // Act
            var result = await _service.GetProductsByWinnerId(200);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(200, result[0].WinnerId);
        }
    }
}