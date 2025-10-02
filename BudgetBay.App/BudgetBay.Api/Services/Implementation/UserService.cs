/*
Name: UserService.cs
Purpose: Service layer for user-related operations, interacting with the user repository.
Parent Class: IUserService.cs
*/
using BudgetBay.Models;
using BudgetBay.Repositories;

namespace BudgetBay.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger; // declare logger
        private readonly IUserRepository _userRepo; // declare user repository
        private readonly IAddressRepository _addressRepo;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository, IAddressRepository addressRepository)
        {
            _logger = logger;
            _userRepo = userRepository;
            _addressRepo = addressRepository;
        }

        public async Task<User?> GetUserInfo(int id)
        {
            _logger.LogInformation($"Getting user info for user with ID: {id}");
            return await _userRepo.GetByIdAsync(id); // call user repo to get user by id
        }
        public async Task<User> CreateUser(User newUser)
        {
            _logger.LogInformation($"Creating new user with username: {newUser.Username}");
            return await _userRepo.AddAsync(newUser); // call user repo to add new user
           
        }
        public async Task<User?> UpdateUser(User updatedUser)
        {
            _logger.LogInformation($"Updating user with ID: {updatedUser.Id}");
            return await _userRepo.UpdateAsync(updatedUser); // call user repo to update user
        }
        public async Task<bool> UsernameExists(string username)
        {
            _logger.LogInformation($"Checking if username exists: {username}");

            return await _userRepo.UsernameExistsAsync(username); // Any will return true if there is a User in the data base with a similar username
        }
        public async Task<bool> EmailExists(string email)
        {
            _logger.LogInformation($"Checking if email exists: {email}");
            return await _userRepo.EmailExistsAsync(email); // Any will return true if there is a User in the data base with a similar email
        }
        public async Task<Address?> UpdateAddress(Address updatedAddress)
        {
            _logger.LogInformation($"Updating address with ID: {updatedAddress.Id}");
            return await _addressRepo.UpdateAsync(updatedAddress); // Update the address to the repo
        }

        public async Task<bool> Exists(int id)
        {
            return await _userRepo.UserExistsAsync(id);
        }
    }
}