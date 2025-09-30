using BudgetBay.Api.Models;

namespace BudgetBay.Api.Repositories.Interface


{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllAsync();
        public Task<Product> GetByIdAsync(int id);

        public Task<Product> AddAsync(Product product);

        public Task<Product> UpdateAsync(Product product);

        public Task DeleteAsync(int id);



    }
}
