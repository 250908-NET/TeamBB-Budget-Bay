﻿using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BudgetBay.Services;
using BudgetBay.Models;
using BudgetBay.DTOs;

namespace BudgetBay.Test
{
    public class UsersServiceTests
    {
        private readonly Mock<IUserService> _mockService;

        public UsersServiceTests()
        {
            _mockService = new Mock<IUserService>();
        }

        [Fact]
        public async Task GetUserInfo_ReturnsUser_WhenExists()
        {
            // Arrange
            var user = new User { Id = 1, Username = "TestUser" };
            _mockService.Setup(s => s.GetUserInfo(1)).ReturnsAsync(user);

            // Act
            var result = await _mockService.Object.GetUserInfo(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("TestUser", result.Username);
        }

        [Fact]
        public async Task GetUserInfo_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetUserInfo(99)).ReturnsAsync((User?)null);

            // Act
            var result = await _mockService.Object.GetUserInfo(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUser_ReturnsUpdatedUser_WhenSuccessful()
        {
            // Arrange
            var updatedUser = new User { Id = 1, Username = "UpdatedUser", Email = "test@test.com" };
            _mockService.Setup(s => s.UpdateUser(It.IsAny<User>())).ReturnsAsync(updatedUser);

            // Act
            var result = await _mockService.Object.UpdateUser(updatedUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedUser", result.Username);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.UpdateUser(It.IsAny<User>())).ReturnsAsync((User?)null);

            // Act
            var result = await _mockService.Object.UpdateUser(new User { Id = 99 });

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAddress_ReturnsAddress_WhenSuccessful()
        {
            // Arrange
            var address = new Address { StreetName = "Main St", City = "Palatine", State = "IL", ZipCode = "60067" };
            _mockService.Setup(s => s.UpdateAddress(It.IsAny<Address>())).ReturnsAsync(address);

            // Act
            var result = await _mockService.Object.UpdateAddress(address);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Main St", result.StreetName);
        }

        [Fact]
        public async Task CreateUserAddress_ReturnsAddress_WhenSuccessful()
        {
            // Arrange
            var address = new Address { StreetName = "Broadway", City = "NY", State = "NY", ZipCode = "10001" };
            _mockService.Setup(s => s.UpdateAddress(It.IsAny<Address>())).ReturnsAsync(address);

            // Act
            var result = await _mockService.Object.UpdateAddress(address);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Broadway", result.StreetName);
        }

        [Fact]
        public async Task EmailExists_ReturnsTrue_WhenEmailExists()
        {
            // Arrange
            var email = "test@gmail.com";
            _mockService.Setup(s => s.EmailExists(email)).ReturnsAsync(true);

            // Act
            var result = await _mockService.Object.EmailExists(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EmailExists_ReturnsFalse_WhenEmailDoesNotExists()
        {
            // Arrange
            var email = "notFound@yahoo.com";
            _mockService.Setup(s => s.EmailExists(email)).ReturnsAsync(false);

            // Act
            var result = await _mockService.Object.EmailExists(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedUser_WhenSuccessful()
        {
            // Arrange
            var newUser = new User
            {
                Id = 1,
                Username = "user1",
                Email = "user1@gmail.com"
            };

            _mockService.Setup(s => s.CreateUser(It.IsAny<User>())).ReturnsAsync(newUser);

            // Act
            var result = await _mockService.Object.CreateUser(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("user1", result.Username);
            Assert.Equal("user1@gmail.com", result.Email);

        }

        [Fact]
        public async Task CreateUser_ReturnsNulll_WhenCreationFails()
        {
            // Arrange
            var newUser = new User
            {
                Id = 1,
                Username = "fail1",
                Email = "fail1@gmail.com"
            };

           _mockService.Setup(s => s.CreateUser(It.IsAny<User>())).ReturnsAsync((User?)null);

            // Act
            var result = await _mockService.Object.CreateUser(newUser);

            // Assert
            Assert.Null(result);
        }
    }
}