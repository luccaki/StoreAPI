using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Models;

namespace StoreAPI.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly AppDbContext _context;

        public StoreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Store> CreateAsync(Store store)
        {
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
            return store;
        }

        public async Task<Store> GetByIdAsync(long companyId, long id)
        {
            var store = await _context.Stores
                .Where(s => s.CompanyId == companyId && s.Id == id)
                .Include(s => s.Products)
                .FirstOrDefaultAsync();

            if (store is null)
                throw new KeyNotFoundException($"Store with ID {id} in Company {companyId} not found");

            return store;
        }

        public async Task UpdateAsync(Store store)
        {
            _context.Stores.Update(store);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Store store)
        {
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Store>> GetAllAsync(long companyId)
        {
            return await _context.Stores
                .Where(s => s.CompanyId == companyId)
                .ToListAsync();
        }
    }
}