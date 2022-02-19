using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VeganStore.Models.Entities;
using VeganStore.Web.API.Data;

namespace VeganStore.Web.API.Repository
{
    public interface IOrderDetailService : IAsyncRepository<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>> GetAllWhereAsync(Expression<Func<OrderDetail, bool>> filter);
    }
    public class OrderDetailService : IOrderDetailService
    {
        private readonly AppDbContext _db;
        public OrderDetailService(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(OrderDetail entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _db.OrderDetails.Include(x => x.Order).Include(x => x.Product).ToListAsync();
        }

        public async Task<IEnumerable<OrderDetail>> GetAllWhereAsync(Expression<Func<OrderDetail, bool>> filter)
        {
            IQueryable<OrderDetail> query = _db.OrderDetails.Include(x => x.Order).Include(x => x.Product);
            query = query.Where(filter);
            return await query.ToListAsync();
        }

        public async Task<OrderDetail> GetByIdAsync(int id)
        {
            return await _db.OrderDetails.Where(x => x.Id == id).Include(x => x.Product).Include(x => x.Order).FirstOrDefaultAsync();
        }

        public async Task<OrderDetail> GetFirstWhereAsync(Expression<Func<OrderDetail, bool>> filter)
        {
            IQueryable<OrderDetail> query = _db.OrderDetails.Include(x => x.Order).Include(x => x.Product);
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(OrderDetail entity)
        {
            _db.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderDetail entity)
        {
            _db.Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
