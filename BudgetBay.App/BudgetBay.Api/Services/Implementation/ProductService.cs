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

        public Task<List<Product>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all products");
            return _productRepository.GetAllAsync();
        }

        public Task<List<Product>> GetActiveProductsAsync()
        {
            _logger.LogInformation("Fetching active products");
            return _productRepository.GetActiveProductsAsync();
        }

        public Task<Product?> GetByIdAsync(int productId)
        {
            _logger.LogInformation($"Fetching product with ID: {productId}");
            return _productRepository.GetByIdAsync(productId);
        }

        public Task<Product?> SearchProductsAsync(string query)
        {
            _logger.LogInformation($"Searching products with query: {query}");
            return _productRepository.SearchProductsAsync(query);
        }

        public Task<Product> CreateProductAsync(Product product)
        {
            _logger.LogInformation($"Creating product: {product.Name}");
            return _productRepository.CreateProductAsync(product);
        }

        public Task<bool> DeleteProductByIdAsync(int producttId)
        {
            _logger.LogInformation($"Deleting product with ID: {producttId}");
            return _productRepository.DeleteProductByIdAsync(producttId);
        }

        public Task<List<Product>> GetProductsBySellerId(int sellerId)
        {
            _logger.LogInformation($"Fetching products for seller ID: {sellerId}");
            return _productRepository.GetProductsBySellerId(sellerId);
        }

        public Task<List<Product>> GetProductsByWinnerId(int winnerId)
        {
            _logger.LogInformation($"Fetching products for winner ID: {winnerId}");
            return _productRepository.GetProductsByWinnerId(winnerId);
        }

        public Task<Product> UpdateProductAsync(int productId, double price)
        {
            _logger.LogInformation($"Updating product ID: {productId} with new price: {price}");
            return _productRepository.UpdateProductAsync(productId, price);
        }
    }
}