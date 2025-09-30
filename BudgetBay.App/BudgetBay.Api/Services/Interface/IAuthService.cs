/*
Name: IAuthService.cs
Purpose: Service layer interface for authentication-related operations.
Child Class: AuthService.cs
*/
using BudgetBay.Models;


namespace BudgetBay.Services
{
    public interface IAuthService
    {
        public Task Login(string username, string password); // Handle's Login methods
        public Task Register(User user); // Creates a new user

    }
}