using BudgetBay.Models;
using BudgetBay.DTOs;

namespace BudgetBay.Services
{
    public interface IUserService
    {
        Task<User> Login(LoginDto login);
        Task<User> Register(RegisterDto register);
        Task UpdateInfo(RegisterDto updateUser);

        Task UpdateAddress(AddressDto updateAddress);

        Task<string> HashPassword(string password);
        Task<bool> CheckPassword(string password, string hashedPassword);
    }
}
