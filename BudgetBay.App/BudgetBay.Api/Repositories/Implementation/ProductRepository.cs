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


        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Bid?> GetHighestBidAsync(int productId)
        {
            string query = "SELECT TOP 1 * FROM Bids WHERE ProductId = {0} ORDER BY Amount DESC";
            return await _context.Bids.FromSqlRaw(query, productId).FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

    }
}
