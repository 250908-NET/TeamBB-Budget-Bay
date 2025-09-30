using BudgetBay.Models;

namespace BudgetBay.Repositories
{
    public interface IBidRepository
    {
        Task<List<Bid>> GetAllAsync();
        Task<Bid?> GetByIdAsync(int id);
        Task AddAsync(Bid bid);
        Task DeleteAsync(Bid bid);
        Task SaveChangesAsync();
    }
}