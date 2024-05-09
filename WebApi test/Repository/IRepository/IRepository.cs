using System.Linq.Expressions;
using WebApi_test.Model;

namespace WebApi_test.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> AllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(T entity);

        Task RemoveAsync(T entity);
        Task SavechangesAsync();
    }
}
