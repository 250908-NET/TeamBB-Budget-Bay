using BudgetBay.Data;
using BudgetBay.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BudgetBay.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _context.Products.ToListAsync();
        }


        public Task<Product?> GetByIdAsync(int id)
        {
            return _context.Products.FirstOrDefaultAsync(product => product.Id == id);
        }

        public async Task<Product> UpdateProductAsync(int productId, double price)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }
            product.CurrentPrice = (decimal)price;
            await _context.SaveChangesAsync();
            return product;
        }

        // Overloaded method to update the entire product
        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
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

        public Task<List<Product>> GetActiveProductsAsync()
        {
            var currentTime = DateTime.UtcNow;
            return _context.Products
                .Where(p => p.EndTime > currentTime)
                .ToListAsync();
        }

        public Task<List<Product>> SearchProductsAsync(string query)
        {
            return _context.Products
                .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                .ToListAsync();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
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
