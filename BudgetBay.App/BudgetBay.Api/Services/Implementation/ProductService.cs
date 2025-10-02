using BudgetBay.Models;
using BudgetBay.Repositories;

namespace BudgetBay.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _productRepository;

        public ProductService(ILogger<ProductService> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public Task<List<Product>> GetAllAsync() => _productRepository.GetAllAsync();
        public Task<List<Product>> GetActiveProductsAsync() => _productRepository.GetActiveProductsAsync();
        public Task<Product?> GetByIdAsync(int productId) => _productRepository.GetByIdAsync(productId);
        public Task<Product?> SearchProductsAsync(string query)=> _productRepository.SearchProductsAsync(query);

        public Task<Product> CreateProductAsync(Product product) => _productRepository.CreateProductAsync(product);
        //public Task<Product> UpdateProductAsync(UpdateProductDto updateProductDto);

        public Task<bool> DeleteProductByIdAsync(int producttId) => _productRepository.DeleteProductByIdAsync(producttId);

        public Task<List<Product>> GetProductsBySellerId(int sellerId) => _productRepository.GetProductsBySellerId(sellerId);

        public Task<List<Product>> GetProductsByWinnerId(int winnerId) => _productRepository.GetProductsByWinnerId(winnerId);

        public Task<Product?> UpdateProductAsync(int productId, double price) => _productRepository.UpdateProductAsync(productId, price);
    }

}