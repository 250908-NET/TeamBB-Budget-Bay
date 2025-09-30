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

        public async Task AddAsync(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
        }

        public async Task DeleteAsync(Bid bid)
        {
            _context.Bids.Remove(bid);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}