using BudgetBay.Models;
namespace BudgetBay.Repositories
{
    public interface IUserRepository
    {
        public Task AddAsync(User user);
        public Task UpdateAsync(User user);
        public Task DeleteAsync(int id);
        public Task<List<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(int id);
        public Task SaveChangesAsync();
    }
}