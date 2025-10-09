﻿using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BudgetBay.Services;
using BudgetBay.Models;
using BudgetBay.Repositories;
using Microsoft.Extensions.Logging;
using AutoMapper;
using BudgetBay.DTOs;

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
            var product = new Product { Id = 3, Name = "Tablet" };
            var dto = new ProductDetailDto { Id = 3, Name = "Tablet" };

            _mockProductRepo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(product);
            _mapper.Setup(m => m.Map<ProductDetailDto>(product)).Returns(dto);

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

            _mockProductRepo.Setup(s => s.SearchProductsAsync("Camera")).ReturnsAsync(products);

            // Act
            var result = await _service.SearchProductsAsync("Camera");

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
            _mockProductRepo.Setup(s => s.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(createdProduct);

            // Act
            var result = await _service.CreateProductAsync(newProductDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Airpods", result.Name);
        }

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
        public async Task DeleteProductById_ReturnsFalse_WhenProductNotFound()
        {
            // Arrange
            _mockProductRepo.Setup(s => s.DeleteProductByIdAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteProductByIdAsync(99);

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

            _mockProductRepo.Setup(s => s.GetProductsBySellerId(100)).ReturnsAsync(products);

            // Act
            var result = await _service.GetProductsBySellerId(100);

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

            _mockProductRepo.Setup(s => s.UpdateProductAsync(9, 9.35)).ReturnsAsync(updatedProduct);

            // Act
            var result = await _service.UpdateProductAsync(9, 9.35);

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

            _mockProductRepo.Setup(s => s.GetProductsByWinnerId(200)).ReturnsAsync(products);

            // Act
            var result = await _service.GetProductsByWinnerId(200);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(200, result[0].WinnerId);
        }

        [Fact]
        public async Task UpdateProductAsync_ReturnsUpdatedProduct_WhenSuccessful()
        {
            // Arrange
            var productId = 1;
            var existingProduct = new Product { Id = productId, Name = "Old Name" };
            var updatedProduct = new Product { Id = productId, Name = "New Name" };
            var updateDto = new UpdateProductDto
            {
                Name = "New Name",
                EndTime = DateTime.UtcNow.AddHours(1)
            };

            _mockProductRepo.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            // _mapper.Setup(m => m.Map(updateDto, existingProduct))
            //     .Callback<UpdateProductDto, Product>((src, dest) =>
            //     {
            //         dest.Name = src.Name;
            //         dest.EndTime = src.EndTime;
            //     });

            _mockProductRepo.Setup(r => r.UpdateProductAsync(existingProduct))
                .ReturnsAsync(updatedProduct);

            // Act
            var result = await _service.UpdateProductAsync(productId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
            _mockProductRepo.Verify(r => r.GetByIdAsync(productId), Times.Once);
            _mockProductRepo.Verify(r => r.UpdateProductAsync(existingProduct), Times.Once);
        }
        
        [Fact]
        public async Task UpdateProductAsync_ThrowsKeyNotFound_WhenProductNotFound()
        {
            // Arrange
            var productId = 99;
            var updateDto = new UpdateProductDto { EndTime = DateTime.UtcNow.AddHours(1) };

            _mockProductRepo.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.UpdateProductAsync(productId, updateDto));

            Assert.Contains("Product with ID 99 not found", ex.Message);
            _mockProductRepo.Verify(r => r.GetByIdAsync(productId), Times.Once);
            _mockProductRepo.Verify(r => r.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task UpdateProductAsync_ThrowsArgumentException_WhenEndTimeInPast()
        {
            // Arrange
            var productId = 2;
            var existingProduct = new Product { Id = productId, Name = "Old Name" };
            var updateDto = new UpdateProductDto
            {
                Name = "Invalid Update",
                EndTime = DateTime.UtcNow.AddMinutes(-5)
            };

            _mockProductRepo.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.UpdateProductAsync(productId, updateDto));

            Assert.Contains("End time must be in future", ex.Message);
            _mockProductRepo.Verify(r => r.GetByIdAsync(productId), Times.Once);
            _mockProductRepo.Verify(r => r.UpdateProductAsync(It.IsAny<Product>()), Times.Never);
        }

    }
}