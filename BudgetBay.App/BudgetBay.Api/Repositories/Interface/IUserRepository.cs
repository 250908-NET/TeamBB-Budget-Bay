using BudgetBay.Models;
namespace BudgetBay.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> AddAsync(User user);
        public Task<User?> UpdateAsync(User user);
        public Task DeleteAsync(int id);
        public Task<List<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(int id);
        public Task<bool> UsernameExistsAsync(string username);
        public Task<bool> EmailExistsAsync(string email);
        public Task SaveChangesAsync();
    }
}