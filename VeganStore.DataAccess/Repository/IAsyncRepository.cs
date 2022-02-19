using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganStore.DataAccess.Repository
{
    public interface IAsyncRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T obj);
        Task RemoveAsync(T entity);

        Task<T> GetFirstOrDefault(int id);
        Task<IEnumerable<T>> GetAllAsync();

        //Task RemoveRangeAsync(IEnumerable<T> entity);
        //Task<T> GetById(int id);
        //Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

        //Task<int> CountAll();
        //Task<int> CountWhere(Expression<Func<T, bool>> predicate);
    }
}
