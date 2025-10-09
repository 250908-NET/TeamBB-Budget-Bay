using BudgetBay.DTOs;
using BudgetBay.Models;

namespace BudgetBay.Services

{
    public interface IProductService
    {

        public Task<List<Product>> GetAllAsync();
        public Task<List<Product>> GetActiveProductsAsync();
        public Task<ProductDetailDto?> GetByIdAsync(int productId);
        public Task<List<Product>> SearchProductsAsync(string query);
        public Task<Product> CreateProductAsync(CreateProductDto product);
        public Task<bool> DeleteProductByIdAsync(int producttId);
        public Task<List<Product>> GetProductsBySellerId(int sellerId);
        public Task<List<Product>> GetProductsByWinnerId(int winnerId);
        public Task<Product?> UpdateProductAsync(int productId, double price);
        public Task<Product?> UpdateProductAsync(int productId, UpdateProductDto productDto);
    }
}
