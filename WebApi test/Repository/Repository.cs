using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi_test.Data;
using WebApi_test.Model;
using WebApi_test.Repository.IRepository;


namespace WebApi_test.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly DbSet<T> dbSet;
        public Repository(ApplicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
            this.dbSet = _dbcontext.Set<T>();
        }

        public async Task<List<T>> AllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {

                    query = query.Include(property);
                }
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                // skip (1-1 * 3 = 0) take 3 records
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            return await query.ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SavechangesAsync();

        }



        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {

                if (tracked == false)
                {
                    query = query.AsNoTracking().Where(filter);
                }
                else
                {
                    query = query.Where(filter);
                }
            }
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {

                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SavechangesAsync();
        }

        public async Task SavechangesAsync()
        {
            await _dbcontext.SaveChangesAsync();
        }


    }
}
