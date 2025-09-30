/*
Name: AuthService.cs
Purpose: Service layer for authentication-related operations, interacting with the user repository.
Parent Class: IAuthService.cs
*/
using BudgetBay.Models;
using BudgetBay.Repositories;


namespace BudgetBay.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUserRepository _userRepository;
        public AuthService(ILogger<AuthService> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public Task Login(string username, string password)
        {
            throw new NotImplementedException();
        }
        public Task Register(User user)
        {
            throw new NotImplementedException();
        }
        private string _HashPassword(string password)
        {
            throw new NotImplementedException();
        }
        private Task<bool> _CheckPassword(string password, string hashedPassword)
        {
            throw new NotImplementedException();
        }


    }
}