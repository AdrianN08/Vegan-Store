using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeganStore.DataAccess.Data;
using VeganStore.Models.Entities;

namespace VeganStore.DataAccess.Repository.Services
{
    public interface IProductService : IAsyncRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllBySubCategoryAsync(string subcategory);
    }
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(Product entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllBySubCategoryAsync(string subcategory)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetFirstOrDefault(int id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Product obj)
        {
            throw new NotImplementedException();
        }
    }
}
