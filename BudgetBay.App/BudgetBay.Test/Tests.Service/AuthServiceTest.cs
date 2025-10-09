using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using BudgetBay.Services;
using BudgetBay.Repositories;
using BudgetBay.Models;
using BudgetBay.DTOs;
using System.Threading.Tasks;

namespace BudgetBay.Test
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<ILogger<AuthService>> _mockLogger;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockConfig = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<AuthService>>();

            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("ThisIsMySuperSecretKeyForTestingAtLeast32BytesLong");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

            _authService = new AuthService(
                _mockLogger.Object,
                _mockUserRepo.Object,
                _mockConfig.Object
            );
        }

        [Fact]
        public async Task Register_ShouldReturnNewUser_WhenCredentialsAreUnique()
        {
            // Arrange
            var registerDto = new RegisterUserDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123"
            };

            // Setup mocks to simulate a new, unique user
            _mockUserRepo.Setup(repo => repo.EmailExistsAsync(registerDto.Email!)).ReturnsAsync(false);
            _mockUserRepo.Setup(repo => repo.UsernameExistsAsync(registerDto.Username!)).ReturnsAsync(false);
            
            // When AddAsync is called with any User object, return that user back with an ID to simulate a successful database insert.
            _mockUserRepo.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                         .ReturnsAsync((User user) => {
                             user.Id = 1; // Assign an ID as a real DB would
                             return user;
                         });

            // Act
            var result = await _authService.Register(registerDto);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be(registerDto.Username);
            result.Email.Should().Be(registerDto.Email);
            result.Id.Should().Be(1);
            
            // Verify that the AddAsync method was called exactly once, confirming the user was saved.
            _mockUserRepo.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Register_ShouldReturnNull_WhenEmailAlreadyExists()
        {
            // Arrange
            var registerDto = new RegisterUserDto
            {
                Username = "testuser",
                Email = "existing@example.com",
                Password = "password123"
            };

            // Setup mock to simulate that the email already exists in the database.
            _mockUserRepo.Setup(repo => repo.EmailExistsAsync(registerDto.Email!)).ReturnsAsync(true);

            // Act
            var result = await _authService.Register(registerDto);

            // Assert
            result.Should().BeNull();
            
            // Verify that we never tried to add the user to the database because the check failed early.
            _mockUserRepo.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Register_ShouldReturnNull_WhenUsernameAlreadyExists()
        {
            // Arrange
            var registerDto = new RegisterUserDto
            {
                Username = "existinguser",
                Email = "test@example.com",
                Password = "password123"
            };

            // Setup mocks to simulate a unique email but a username that already exists.
            _mockUserRepo.Setup(repo => repo.EmailExistsAsync(registerDto.Email!)).ReturnsAsync(false);
            _mockUserRepo.Setup(repo => repo.UsernameExistsAsync(registerDto.Username!)).ReturnsAsync(true);

            // Act
            var result = await _authService.Register(registerDto);

            // Assert
            result.Should().BeNull();
            _mockUserRepo.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Login_ShouldReturnJwtToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDto = new LoginUserDto
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Hash the password to store in the mock user object, simulating a real user record.
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(loginDto.Password!);
            var existingUser = new User
            {
                Id = 1,
                Email = loginDto.Email!,
                PasswordHash = hashedPassword
            };

            // Setup mock to return the user when searched by email.
            _mockUserRepo.Setup(repo => repo.GetByEmailAsync(loginDto.Email!)).ReturnsAsync(existingUser);

            // Act
            var token = await _authService.Login(loginDto);

            // Assert
            token.Should().NotBeNullOrEmpty();
            // A valid JWT token has three parts separated by two periods.
            token.Split('.').Should().HaveCount(3); 
        }

        [Fact]
        public async Task Login_ShouldReturnNull_WhenPasswordIsInvalid()
        {
            // Arrange
            var loginDto = new LoginUserDto
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            // The user exists, but their stored password hash corresponds to "correctpassword", not "wrongpassword".
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword");
            var existingUser = new User
            {
                Id = 1,
                Email = loginDto.Email!,
                PasswordHash = hashedPassword
            };

            _mockUserRepo.Setup(repo => repo.GetByEmailAsync(loginDto.Email!)).ReturnsAsync(existingUser);

            // Act
            var token = await _authService.Login(loginDto);

            // Assert
            token.Should().BeNull();
        }

        [Fact]
        public async Task Login_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var loginDto = new LoginUserDto
            {
                Email = "nonexistent@example.com",
                Password = "password123"
            };

            // Setup mock to return null, simulating a user not found in the database.
            _mockUserRepo.Setup(repo => repo.GetByEmailAsync(loginDto.Email!)).ReturnsAsync((User?)null);

            // Act
            var token = await _authService.Login(loginDto);

            // Assert
            token.Should().BeNull();
        }
    }
}