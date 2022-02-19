using System.Linq.Expressions;

namespace VeganStore.Web.API.Repository
{
    public interface IAsyncRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);      
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetFirstWhereAsync(Expression<Func<T, bool>> filter);
        
        //Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null, string category = null, string subcategory = null);

        //Task RemoveRangeAsync(IEnumerable<T> entity);
        //Task<T> GetFirstOrDefault(int id);
        //Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);

        //Task<int> CountAll();
        //Task<int> CountWhere(Expression<Func<T, bool>> predicate);
    }
}
