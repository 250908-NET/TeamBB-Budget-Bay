
using BudgetBay.Models;

namespace BudgetBay.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IAddressRepository _addressRepo;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository, IAddressRepository addressRepository)
        {
            _logger = logger;
            _userRepo = userRepository;
            _addressRepo = addressRepository;
        }

        public async Task<User> GetUserInfo(int id)
        {
            return await _userRepo.GetByIdAsync(id);
        }
        public async Task<User> CreateUser(User newUser)
        {
            return await _userRepo.AddAsync(newUser);
        }
        public async Task<User> UpdateUser(User updatedUser)
        {
            return await _userRepo.UpdateAsync(updatedUser);
        }
        public async Task<bool> UsernameExists(string username)
        {
            return await _userRepo.GetAllAsync().Any(u => u.Username == username); // Any will return true if there is a User in the data base with a similar username
        }
        public async Task<bool> EmailExists(string email)
        {
            return await _userRepo.GetAllAsync().Any(u => u.Email == email); // Any will return true if there is a User in the data base with a similar email
        }
        public async Task<Address> UpdateAddress(Address updatedAddress)
        {
            return await _addressRepo.UpdateAsync(updatedAddress); // Update the address to the repo
        }
    }
}