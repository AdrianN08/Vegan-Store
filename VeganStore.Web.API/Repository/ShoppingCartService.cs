using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VeganStore.Models.Entities;
using VeganStore.Web.API.Data;

namespace VeganStore.Web.API.Repository
{
    public interface IShoppingCartService : IAsyncRepository<ShoppingCart>
    {
        Task<int> DecrementCount(ShoppingCart shoppingCart, int count);
        Task<int> IncrementCount(ShoppingCart shoppingCart, int count);
        Task<IEnumerable<ShoppingCart>> GetAllWhereAsync(Expression<Func<ShoppingCart, bool>> filter);
        Task RemoveRangeAsync(IEnumerable<ShoppingCart> entity);
    }
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _db;
        public ShoppingCartService(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(ShoppingCart entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<int> DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Quantity -= count;
            _db.Update(shoppingCart);
            await _db.SaveChangesAsync();
            return shoppingCart.Quantity;
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllAsync()
        {
            return await _db.ShoppingCarts.Include(x => x.Product).ToListAsync();
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllWhereAsync(Expression<Func<ShoppingCart, bool>> filter)
        {
            IQueryable<ShoppingCart> query = _db.ShoppingCarts.Include(x => x.Product);
            query = query.Where(filter);
            return await query.ToListAsync();
        }

        public async Task<ShoppingCart> GetByIdAsync(int id)
        {
            return await _db.ShoppingCarts.Where(x => x.Id == id).Include(x => x.Product).FirstOrDefaultAsync();
        }

        public async Task<ShoppingCart> GetFirstWhereAsync(Expression<Func<ShoppingCart, bool>> filter)
        {
            IQueryable<ShoppingCart> query = _db.ShoppingCarts.Include(x => x.Product);
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Quantity += count;
            _db.Update(shoppingCart);
            await _db.SaveChangesAsync();
            return shoppingCart.Quantity;
        }

        public async Task RemoveAsync(ShoppingCart entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<ShoppingCart> entity)
        {
            _db.RemoveRange(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(ShoppingCart entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
