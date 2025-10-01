using BudgetBay.Models;

namespace BudgetBay.Repositories


{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllAsync();
        public Task<Product> GetByIdAsync(int id);

        public Task<Product> AddAsync(Product product);

        public Task<Product> UpdateAsync(Product product);

        public Task<Bid?> GetHighestBidAsync(int productId);

        public Task DeleteAsync(int id);





    }
}
