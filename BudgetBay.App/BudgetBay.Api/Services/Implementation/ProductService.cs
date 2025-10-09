using AutoMapper;
using BudgetBay.DTOs;
using BudgetBay.Models;
using BudgetBay.Repositories;


namespace BudgetBay.Services
{
    public class ProductService(ILogger<ProductService> logger, IProductRepository productRepository, IMapper mapper) : IProductService
    {
        private readonly ILogger<ProductService> _logger = logger;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;

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

        public async Task<ProductDetailDto?> GetByIdAsync(int productId)
        {
            _logger.LogInformation($"Fetching product with ID: {productId}");
            var product = await _productRepository.GetByIdAsync(productId);
            return _mapper.Map<ProductDetailDto>(product);
        }

        public Task<List<Product>> SearchProductsAsync(string query)
        {
            _logger.LogInformation($"Searching products with query: {query}");
            return _productRepository.SearchProductsAsync(query);
        }

        public async Task<Product> CreateProductAsync(CreateProductDto productDto)
        {
            _logger.LogInformation($"Creating product: {productDto.Name}");

            if(productDto.EndTime <= DateTime.UtcNow)
            {
                throw new ArgumentException("End time must be in future");
            }

            var product = _mapper.Map<Product>(productDto);
            return await _productRepository.CreateProductAsync(product);
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

        public Task<Product?> UpdateProductAsync(int productId, double price)
        {
            _logger.LogInformation($"Updating product with ID: {productId} to new price: {price}");
            return _productRepository.UpdateProductAsync(productId, price);
        }

        public async Task<Product?> UpdateProductAsync(int productId, UpdateProductDto productDto)
        {
            _logger.LogInformation($"Updating product with ID: {productId}");
            var currentProduct = await _productRepository.GetByIdAsync(productId);

            if (currentProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found");
            }

            if (productDto.EndTime <= DateTime.UtcNow)
            {
                throw new ArgumentException("End time must be in future");
            }

            _mapper.Map(productDto, currentProduct);
            return await _productRepository.UpdateProductAsync(currentProduct);

        }
    }
}