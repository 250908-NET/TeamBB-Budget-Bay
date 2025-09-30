using BudgetBay.Models;

namespace BudgetBay.Repositories
{
    public interface IAddressRepository
    {
        public Task<List<Address>> GetAllAsync();
        public Task<Address?> GetByIdAsync(int id);
        public Task<Address> AddAsync(Address address);
        public Task UpdateAsync(Address address);
        public Task DeleteAsync(int id);
        public Task SaveChangesAsync();
    }
}