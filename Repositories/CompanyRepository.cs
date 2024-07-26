using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Models;

namespace StoreAPI.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Company> CreateAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> GetByIdAsync(long id)
        {
            var company = await _context.Companies.Include(c => c.Stores).FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
                throw new KeyNotFoundException($"Company with ID {id} not found");

            return company;
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Company company)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }
    }
}