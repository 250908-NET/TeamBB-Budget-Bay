using BudgetBay.Models;
using BudgetBay.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetBay.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User?> AddAsync(User user)
        {
            var entityEntry = await _context.Users.AddAsync(user);
            await SaveChangesAsync();
            return entityEntry.Entity;

        }
        public async Task<User?> UpdateAsync(User user)
        {
            var entityEntry =_context.Users.Update(user);
            await SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}