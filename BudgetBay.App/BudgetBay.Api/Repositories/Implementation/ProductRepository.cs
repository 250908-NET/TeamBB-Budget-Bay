using BudgetBay.Models;
using BudgetBay.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetBay.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }


        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(product => product.Id == id);
        }

        public async Task<Product> UpdateProductAsync(int productId, double price)
        {
            _context.Products.Update(new Product { Id = productId, Bids = new List<Bid>(), CurrentPrice = (decimal)price });
            await _context.SaveChangesAsync();
            return await GetByIdAsync(productId);
        }

        public async Task<bool> DeleteProductByIdAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Product>> GetActiveProductsAsync()
        {
            var currentTime = DateTime.UtcNow;
            return await _context.Products
                .Where(p => p.EndTime > currentTime)
                .ToListAsync();
        }

        public Task<Product> SearchProductsAsync(string query)
        {
            return _context.Products.FirstOrDefaultAsync(p => p.Name.Contains(query) || p.Description.Contains(query));

        }
        public Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Task.FromResult(product);

        }

        public Task<List<Product>> GetProductsBySellerId(int sellerId)
        {
            return _context.Products.Where(p => p.SellerId == sellerId).ToListAsync();
        }
        public Task<List<Product>> GetProductsByWinnerId(int winnerId)
        {
            return _context.Products.Where(p => p.WinnerId == winnerId).ToListAsync();
        }

    }
}
