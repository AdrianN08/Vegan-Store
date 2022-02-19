using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VeganStore.Models.Entities;
using VeganStore.Models.SubCategory;
using VeganStore.Web.API.Data;

namespace VeganStore.Web.API.Repository
{
    public interface ISubCategoryService : IAsyncRepository<SubCategory>
    {
        Task<IEnumerable<SubCategory>> GetByCategoriesAsync(Expression<Func<SubCategory, bool>> filter = null, string _category = null);
    }
    public class SubCategoryService : ISubCategoryService
    {
        private readonly AppDbContext _db;


        public SubCategoryService(AppDbContext db, ICategoryService categoryService)
        {
            _db = db;
        }
        public async Task AddAsync(SubCategory entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetAllAsync()
        {
            return await _db.SubCategories.Include(x => x.Category).ToListAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetByCategoriesAsync(Expression<Func<SubCategory, bool>> filter = null, string _category = null)
        {
            IQueryable<SubCategory> query = _db.SubCategories.Include(x => x.Category);
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (_category != null)
            {
                query = query.Where(x => x.Category.Name == _category);

            }
            return await query.ToListAsync();
        }

        public async Task<SubCategory> GetByIdAsync(int id)
        {
            return await _db.SubCategories.Where(x => x.Id == id).Include(x => x.Category).FirstOrDefaultAsync();
        }

        public async Task<SubCategory> GetFirstWhereAsync(Expression<Func<SubCategory, bool>> filter)
        {
            IQueryable<SubCategory> query = _db.SubCategories.Include(x => x.Category);
            query = query.Where(filter);          
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(SubCategory entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(SubCategory entity)
        {
            _db.SubCategories.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
