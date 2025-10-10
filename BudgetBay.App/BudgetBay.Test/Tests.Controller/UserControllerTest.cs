using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Security.Claims;
using System.Security.Principal;
using BudgetBay.Controllers;
using BudgetBay.Services;
using BudgetBay.Models;
using BudgetBay.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace BudgetBay.Test
{
    public class UserControllerTests
    {
        private readonly UsersController _controller;
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<IBidService> _mockBidService;

        public UserControllerTests()
        {
            _mockLogger = new Mock<ILogger<UsersController>>();
            _mockMapper = new Mock<IMapper>();
            _mockUserService = new Mock<IUserService>();
            _mockProductService = new Mock<IProductService>();
            _mockBidService = new Mock<IBidService>();
            _controller = new UsersController(

                _mockLogger.Object,
                _mockMapper.Object,
                _mockUserService.Object,
                _mockProductService.Object,
                _mockBidService.Object
            );
        }

        // Helper to set a fake authenticated user
        private void SetUserClaims(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        private void SetAuthenticatedUser(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.NameIdentifier, userId) }, "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnForbid_WhenUserIdMismatch()
        {
            // Arrange
            SetUserClaims("5");
            int requestedId = 10;

            // Act
            var result = await _controller.GetByIdAsync(requestedId);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            int userId = 5;
            SetUserClaims(userId.ToString());

            var user = new User { Id = userId, Username = "Test User", Email = "example@mail.com" };
            var userDto = new UserDto { Username = user.Username, Email = user.Email };

            _mockUserService.Setup(s => s.GetUserInfo(userId)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            // Act
            var result = await _controller.GetByIdAsync(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDto = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(userDto.Username, returnedDto.Username);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 7;
            SetUserClaims(userId.ToString());

            _mockUserService.Setup(s => s.GetUserInfo(userId)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetByIdAsync(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateUserAddress_UnauthorizedUser_ReturnsForbid()
        {
            // Arrange
            SetAuthenticatedUser("2");
            var dto = new AddressDto();

            // Act
            var result = await _controller.CreateUserAddress(1, dto);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task CreateUserAddress_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            SetAuthenticatedUser("5");
            var dto = new AddressDto();

            _mockUserService.Setup(s => s.GetUserInfo(5)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.CreateUserAddress(5, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateUserAddress_UserAlreadyHasAddress_ReturnsConflict()
        {
            // Arrange
            SetAuthenticatedUser("10");
            var dto = new AddressDto();
            var existingUser = new User { Id = 10, AddressId = 99 };

            _mockUserService.Setup(s => s.GetUserInfo(10)).ReturnsAsync(existingUser);

            // Act
            var result = await _controller.CreateUserAddress(10, dto);

            // Assert
            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("User already has an address. Use PUT to update it.", conflict.Value);
        }

        [Fact]
        public async Task CreateUserAddress_ValidUser_ReturnsOk()
        {
            // Arrange
            SetAuthenticatedUser("15");
            var dto = new AddressDto { StreetNumber = 123, StreetName = "Main St", City = "Springfield" };
            var user = new User { Id = 15 };
            var address = new Address { Id = 1, StreetNumber = "123", StreetName = "Main St", City = "Springfield" };
            var mappedAddress = new Address();

            _mockUserService.Setup(s => s.GetUserInfo(15)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<Address>(dto)).Returns(mappedAddress);
            _mockUserService.Setup(s => s.CreateAddress(mappedAddress)).ReturnsAsync(address);
            _mockUserService.Setup(s => s.UpdateUser(user)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<AddressDto>(address)).Returns(dto);

            // Act
            var result = await _controller.CreateUserAddress(15, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnDto = Assert.IsType<AddressDto>(okResult.Value);
            Assert.Equal(dto.StreetName, returnDto.StreetName);
            Assert.Equal(dto.StreetNumber, returnDto.StreetNumber);
        }

        [Fact]
        public async Task GetAllUserBids_ShouldReturnForbid_WhenUserIdMismatch()
        {
            // Arrange
            SetAuthenticatedUser("3");
            int requestedId = 10;

            // Act
            var result = await _controller.GetAllUserBids(requestedId);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }


        [Fact]
        public async Task GetAllUserBids_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 5;
            SetAuthenticatedUser(userId.ToString());

            _mockUserService.Setup(s => s.GetUserInfo(userId)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetAllUserBids(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllSellersProducts_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 8;
            SetAuthenticatedUser(userId.ToString());
            _mockUserService.Setup(s => s.GetUserInfo(userId)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetAllSellersProducts(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        
    }
}