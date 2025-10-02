using BudgetBay.Models;

namespace BudgetBay.Repositories


{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllAsync();
        public Task<List<Product>> GetActiveProductsAsync();
        public Task<Product?> GetByIdAsync(int productId);
        public Task<Product?> SearchProductsAsync(string query);
        public Task<Product> CreateProductAsync(Product product);
        public Task<bool> DeleteProductByIdAsync(int productId);
        public Task<List<Product>> GetProductsBySellerId(int sellerId);
        public Task<List<Product>> GetProductsByWinnerId(int winnerId);
        public Task<Product?> UpdateProductAsync(int productId, double price);
    }
}
