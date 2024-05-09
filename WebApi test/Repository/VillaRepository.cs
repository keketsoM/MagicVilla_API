using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi_test.Data;
using WebApi_test.Model;
using WebApi_test.Repository.IRepository;
namespace WebApi_test.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbcontext _dbcontext;
        public VillaRepository(ApplicationDbcontext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;

        }
        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdateDate = DateTime.Now;
            _dbcontext.Update(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;

        }


    }
}
