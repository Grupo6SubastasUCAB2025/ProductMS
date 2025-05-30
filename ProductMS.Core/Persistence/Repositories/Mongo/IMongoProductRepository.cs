using ProductMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductMS.Core.Persistence.Repositories.Mongo
{
    public interface IMongoProductRepository
    {
        Task AddAsync(Product product);
        Task<Product> GetByIdAsync(int id);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
    }
}