/*
Name: AuthService.cs
Purpose: Service layer for authentication-related operations, interacting with the user repository.
Parent Class: IAuthService.cs
*/
using BudgetBay.Models;
using BudgetBay.Repositories;
using BudgetBay.DTOs;

using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;


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
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool _CheckPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private string _GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}