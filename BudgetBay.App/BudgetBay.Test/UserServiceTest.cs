﻿using Xunit;
using Moq;
using BudgetBay.Services;
using BudgetBay.Repositories;
using BudgetBay.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class UserServiceTests
{
    private readonly UserService _service;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<IAddressRepository> _mockAddressRepo;
    private readonly Mock<ILogger<UserService>> _mockLogger;

    public UserServiceTests()
    {
        _mockUserRepo = new Mock<IUserRepository>();
        _mockAddressRepo = new Mock<IAddressRepository>();
        _mockLogger = new Mock<ILogger<UserService>>();

        _service = new UserService(
            _mockLogger.Object,
            _mockUserRepo.Object,
            _mockAddressRepo.Object
        );
    }

    [Fact]
    public async Task GetUserInfo_ReturnsUser_WhenExists()
    {
        // Arrange
        var user = new User { Id = 1, Username = "TestUser" };
        _mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

        // Act
        var result = await _service.GetUserInfo(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestUser", result.Username);
    }

    [Fact]
    public async Task GetUserInfo_ReturnsNull_WhenUserDoesNotExist()
    {
        // Arrange
        _mockUserRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        // Act
        var result = await _service.GetUserInfo(99);

        // Assert
        Assert.Null(result);
    }

         [Fact]
        public async Task UpdateUser_ReturnsUpdatedUser_WhenSuccessful()
        {
            // Arrange
            var updatedUser = new User { Id = 1, Username = "UpdatedUser", Email = "test@test.com" };
            _mockUserRepo.Setup(r => r.UpdateAsync(updatedUser)).ReturnsAsync(updatedUser);

            // Act
            var result = await _service.UpdateUser(updatedUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UpdatedUser", result.Username);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var user = new User { Id = 99 };
            _mockUserRepo.Setup(r => r.UpdateAsync(user)).ReturnsAsync((User?)null);

            // Act
            var result = await _service.UpdateUser(user);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAddress_ReturnsAddress_WhenSuccessful()
        {
            // Arrange
            var address = new Address { Id = 1, StreetName = "Main St", City = "Palatine", State = "IL", ZipCode = "60067" };
            _mockAddressRepo.Setup(r => r.UpdateAsync(address)).ReturnsAsync(address);

            // Act
            var result = await _service.UpdateAddress(address);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Main St", result.StreetName);
        }

        [Fact]
        public async Task CreateUserAddress_ReturnsAddress_WhenSuccessful()
        {
            // Arrange
            var address = new Address { Id = 2, StreetName = "Broadway", City = "NY", State = "NY", ZipCode = "10001" };
            _mockAddressRepo.Setup(r => r.AddAsync(address)).ReturnsAsync(address);

            // Act
            var result = await _service.CreateAddress(address);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Broadway", result.StreetName);
        }

        [Fact]
        public async Task EmailExists_ReturnsTrue_WhenEmailExists()
        {
            // Arrange
            var email = "test@gmail.com";
            _mockUserRepo.Setup(r => r.EmailExistsAsync(email)).ReturnsAsync(true);

            // Act
            var result = await _service.EmailExists(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EmailExists_ReturnsFalse_WhenEmailDoesNotExists()
        {
            // Arrange
            var email = "notFound@yahoo.com";
            _mockUserRepo.Setup(r => r.EmailExistsAsync(email)).ReturnsAsync(false);

            // Act
            var result = await _service.EmailExists(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedUser_WhenSuccessful()
        {
            // Arrange
            var newUser = new User { Id = 1, Username = "user1", Email = "user1@gmail.com" };
            _mockUserRepo.Setup(r => r.AddAsync(newUser)).ReturnsAsync(newUser);

            // Act
            var result = await _service.CreateUser(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("user1", result.Username);
            Assert.Equal("user1@gmail.com", result.Email);
        }

        [Fact]
        public async Task CreateUser_ReturnsNull_WhenCreationFails()
        {
            // Arrange
            var newUser = new User { Id = 1, Username = "fail1", Email = "fail1@gmail.com" };
            _mockUserRepo.Setup(r => r.AddAsync(newUser)).ReturnsAsync((User?)null);

            // Act
            var result = await _service.CreateUser(newUser);

            // Assert
            Assert.Null(result);
        }
    }