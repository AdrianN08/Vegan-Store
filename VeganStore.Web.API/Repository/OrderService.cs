using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VeganStore.Models.Entities;
using VeganStore.Web.API.Data;

namespace VeganStore.Web.API.Repository
{
    public interface IOrderService : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllWhereAsync(Expression<Func<Order, bool>> filter);
    }
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _db;

        public OrderService(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Order entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _db.Orders.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllWhereAsync(Expression<Func<Order, bool>> filter)
        {
            IQueryable<Order> query = _db.Orders;
            query = query.Where(filter);
            return await query.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _db.Orders.Where(x=> x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Order> GetFirstWhereAsync(Expression<Func<Order, bool>> filter)
        {
            IQueryable<Order> query = _db.Orders;
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(Order entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
