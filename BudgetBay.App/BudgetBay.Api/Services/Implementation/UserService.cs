
using BudgetBay.Models;
using BudgetBay.DTOs;

namespace BudgetBay.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IAddressRepository _addressRepo;

        public UserService Service(IUserRepository userRepository, IAddressRepository addressRepository)
        {
            _userRepo = userRepository;
            _addressRepo = addressRepository;
        }

        public async Task<User> Login(LoginDto login)
        {
            throw new NotImplementedException();
        }

    }
}