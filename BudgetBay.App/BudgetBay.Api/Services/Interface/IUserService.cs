/*
Name: IUserService.cs
Purpose: Service layer interface for user-related operations.
Child Class: UserService.cs
*/
using BudgetBay.Models;


namespace BudgetBay.Services
{
    public interface IUserService
    {
        Task<User> GetUserInfo(string username); // get user info from username
        Task<User> CreateUser(User newUser); // create new user
        Task<User> UpdateUser(User updateUser); // update user info from Username, Email, and Password

        Task<bool> UsernameExists(string username); // Check if username exists

        Task<bool> EmailExists(string email); // Check if email exists
        Task<Address> UpdateAddress(Address updateAddress); // Updates user's address information


    }
}
