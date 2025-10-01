/*
Name: AuthService.cs
Purpose: Service layer for authentication-related operations, interacting with the user repository.
Parent Class: IAuthService.cs
*/
using BudgetBay.Models;
using BudgetBay.Repositories;
using BudgetBay.DTOs;


namespace BudgetBay.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(
            ILogger<AuthService> logger,
            IUserRepository userRepository,
            IConfiguration config
        )
        {
            _logger = logger;
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<string?> Login(LoginUserDto loginUserDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginUserDto.Email!);
            if (user is null || !_CheckPassword(loginUserDto.Password!, user.PasswordHash))
            {
                _logger.LogWarning("Login failed for email: {Email}", loginUserDto.Email);
                return null;
            }
            _logger.LogInformation("User {Email} logged in successfully.", user.Email);
            return _GenerateJwtToken(user);
        }
        public async Task<User?> Register(RegisterUserDto registerDto)
        {
            if (await _userRepository.EmailExistsAsync(registerDto.Email!) ||
                await _userRepository.UsernameExistsAsync(registerDto.Username!))
            {
                _logger.LogWarning("Registration failed: Email or Username already exists. Email: {Email}, Username: {Username}", registerDto.Email, registerDto.Username);
                return null; // User already exists
            }

            var newUser = new User
            {
                Username = registerDto.Username!,
                Email = registerDto.Email!,
                PasswordHash = _HashPassword(registerDto.Password!)
            };

            var createdUser = await _userRepository.AddAsync(newUser);
            _logger.LogInformation("New user registered successfully with ID: {UserId}", createdUser?.Id);
            return createdUser;
        }
        private string _HashPassword(string password)
        {
            throw new NotImplementedException();
        }
        private bool _CheckPassword(string password, string hashedPassword)
        {
            throw new NotImplementedException();
        }

        private string _GenerateJwtToken(User user)
        {
            return "";
        }


    }
}