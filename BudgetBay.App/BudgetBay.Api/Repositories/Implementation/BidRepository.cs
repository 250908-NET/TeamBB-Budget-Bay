using BudgetBay.Models;
using BudgetBay.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetBay.Repositories
{
    public class BidRepository : IBidRepository
    {

        private readonly AppDbContext _context;

        public BidRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetAllAsync()
        {
            return await _context.Bids
                .Include(b => b.Product)
                .Include(b => b.Bidder)
                .ToListAsync();
        }

        public async Task<Bid?> GetByIdAsync(int id)
        {
            
            return await _context.Bids.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Bid?> AddAsync(Bid bid)
        {
            var entityEntry = await _context.Bids.AddAsync(bid);
            await SaveChangesAsync();
            return entityEntry.Entity;
        }
        public async Task<List<Bid>> GetByProductIdAsync(int productId)
        {
            return await _context.Bids
                .Where(b => b.ProductId == productId)
                .Include(b => b.Product)
                .Include(b => b.Bidder)
                .ToListAsync();
        }

        public async Task<List<Bid>> GetByUserIdAsync(int userId)
        {
            return await _context.Bids
                .Where(b => b.BidderId == userId)
                .Include(b => b.Product)
                .Include(b => b.Bidder)
                .ToListAsync();
        }

        public async Task DeleteAsync(Bid bid)
        {
            _context.Bids.Remove(bid);
            await Task.CompletedTask;
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}