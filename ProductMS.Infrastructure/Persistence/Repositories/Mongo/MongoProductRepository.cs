using MongoDB.Driver;
using ProductMS.Core.Persistence.Repositories.Mongo;
using ProductMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductMS.Infrastructure.Persistence.Repositories.Mongo
{
    public class MongoProductRepository : IMongoProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public MongoProductRepository(IMongoDatabase database)
        {
            _products = database.GetCollection<Product>("Products");
        }

        public async Task AddAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
        }

        public async Task DeleteAsync(int id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }
    }
}