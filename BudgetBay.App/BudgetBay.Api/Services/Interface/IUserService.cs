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
        public Task<User> GetUserInfo(int id); // get user info from id
        public Task<User> CreateUser(User newUser); // create new user
        public Task<User> UpdateUser(User updateUser); // update user info from Username, Email, and Password

        public Task<bool> UsernameExists(string username); // Check if username exists

        public Task<bool> EmailExists(string email); // Check if email exists
        public Task<Address> UpdateAddress(Address updateAddress); // Updates user's address information


    }
}
