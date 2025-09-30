using BudgetBay.Api.Models;

namespace BudgetBay.Api.Repositories.Interface


{
    public interface IProductRepo
    {
        public Task<List<Product>> GetAllProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);

        public Task<Product> AddProductAsync(Product product);

        public Task<Product> UpdateProductAsync(Product product);



    }
}
