using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VeganStore.Models.Entities;
using VeganStore.Web.API.Data;

namespace VeganStore.Web.API.Repository
{
    public interface ICategoryService : IAsyncRepository<Category>
    {
     
    }

    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _db;

        public CategoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Category entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _db.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Category> GetFirstWhereAsync(Expression<Func<Category, bool>> filter)
        {
            IQueryable<Category> query = _db.Categories;
            query = query.Where(filter);           
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(Category entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category entity)
        {
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
