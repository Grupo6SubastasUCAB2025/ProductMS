using ProductMS.Domain.Entities;
using System.Threading.Tasks;

namespace ProductMS.Core.Persistence.Repositories.PostgreSQL
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<Product> GetByIdAsync(int id);
        Task UpdateAsync(Product product);
    }
}