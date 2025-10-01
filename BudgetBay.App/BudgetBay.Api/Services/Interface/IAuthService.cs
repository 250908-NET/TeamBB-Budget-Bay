/*
Name: IAuthService.cs
Purpose: Service layer interface for authentication-related operations.
Child Class: AuthService.cs
*/
using BudgetBay.Models;
using BudgetBay.DTOs;


namespace BudgetBay.Services
{
    public interface IAuthService
    {
        public Task<string?> Login(LoginUserDto loginUserDto); // Handle's Login methods
        public Task<User?> Register(RegisterUserDto registerUserDto); // Creates a new user

    }
}