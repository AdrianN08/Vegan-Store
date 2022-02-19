using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VeganStore.Models.Entities;
using VeganStore.Web.API.Data;

namespace VeganStore.Web.API.Repository
{
    public interface IProductService : IAsyncRepository<Product>
    {
        Task RemoveRangeAsync(IEnumerable<Product> entity);
        Task<IEnumerable<Product>> GetByCategoriesAsync(Expression<Func<Product, bool>> filter = null, string _category = null, string _subcategory = null);
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
            return await _db.Products.Include(x => x.SubCategory).ThenInclude(x => x.Category).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _db.Products.Where(x => x.Id == id).Include(x => x.SubCategory).ThenInclude(x => x.Category).FirstOrDefaultAsync();
        }

        public async Task<Product> GetFirstWhereAsync(Expression<Func<Product, bool>> filter)
        {
            IQueryable<Product> query = _db.Products.Include(x => x.SubCategory).ThenInclude(x => x.Category);
            query = query.Where(filter);            
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(Product entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entity)
        {
            _db.RemoveRange(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoriesAsync(Expression<Func<Product, bool>> filter = null, string _category = null, string _subcategory = null)
        {
            IQueryable<Product> query = _db.Products.Include(x => x.SubCategory).ThenInclude(x => x.Category);
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (_category != null || _subcategory != null)
            {
                if (_category == null)
                {
                    query = query.Where(x => x.SubCategory.Name == _subcategory);
                }
                else if (_subcategory == null)
                {
                    query = query.Where(x => x.SubCategory.Category.Name == _category);
                }
                else
                {
                    query = query.Where(x => x.SubCategory.Name == _subcategory & x.SubCategory.Category.Name == _category);
                }

            }
            return await query.ToListAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _db.Products.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
