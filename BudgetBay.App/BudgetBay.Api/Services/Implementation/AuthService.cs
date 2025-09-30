/*
Name: AuthService.cs
Purpose: Service layer for authentication-related operations, interacting with the user repository.
Parent Class: IAuthService.cs
*/
using BudgetBay.Models;


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

    }
}