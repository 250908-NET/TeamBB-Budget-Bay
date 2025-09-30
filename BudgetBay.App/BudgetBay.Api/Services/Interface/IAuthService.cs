using BudgetBay.Models;


namespace BudgetBay.Services
{
    public interface IAuthService
    {
        Task Login(string username, string password); // Handle's Login methods
        Task Register(User user); // Creates a new user

        string _HashPassword(string password); // Hashes password form register
        Task<bool> CheckPassword(string password, string hashedPassword); // checks if password is correct

    }
}