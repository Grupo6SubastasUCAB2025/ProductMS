using Microsoft.EntityFrameworkCore;
using ProductMS.Core.Persistence.Repositories.PostgreSQL;
using ProductMS.Domain.Entities;
using ProductMS.Infrastructure.Contexts;

namespace ProductMS.Infrastructure.Persistence.Repositories.PostgreSQL
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Productos.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Productos.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}