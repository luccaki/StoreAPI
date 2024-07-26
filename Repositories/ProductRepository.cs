using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Models;
using System.Data;

namespace StoreAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var nameParam = new SqlParameter("@Name", product.Name);
            var valueParam = new SqlParameter("@Value", product.Value);
            var storeIdParam = new SqlParameter("@StoreId", product.StoreId);
            var newProductIdParam = new SqlParameter("@NewProductId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC dbo.InsertProduct @Name, @Value, @StoreId, @NewProductId OUTPUT",
                nameParam, valueParam, storeIdParam, newProductIdParam);

            var id = (int)newProductIdParam.Value;
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);   
        }

        public async Task<Product> GetByIdAsync(long storeId, long id)
        {
            var product = await _context.Products
                .Where(p => p.StoreId == storeId && p.Id == id)
                .FirstOrDefaultAsync();

            if (product is null)
                throw new KeyNotFoundException($"Product with ID {id} in Store {storeId} not found");

            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync(long storeId)
        {
            return await _context.Products
                .Where(p => p.StoreId == storeId)
                .ToListAsync();
        }
        public async Task<string> GetAllProductsAsJsonAsync(long companyId, long storeId)
        {
            var companyIdParam = new SqlParameter("@CompanyId", companyId);
            var storeIdParam = new SqlParameter("@StoreId", storeId);

            return await _context.Database
                .SqlQueryRaw<string>("SELECT dbo.GetProductsJson(@CompanyId, @StoreId) as Value", companyIdParam, storeIdParam)
                .FirstOrDefaultAsync();
        }
    }
}