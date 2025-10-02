using BudgetBay.Models;

namespace BudgetBay.Repositories
{
    public interface IBidRepository
    {
        Task<List<Bid>> GetAllAsync();
        Task<Bid?> GetByIdAsync(int id);
        Task<List<Bid>> GetByProductIdAsync(int productId);
        Task<List<Bid>> GetByUserIdAsync(int userId);
        Task<Bid?> AddAsync(Bid bid);
        Task DeleteAsync(Bid bid);
        Task SaveChangesAsync();
    }
}