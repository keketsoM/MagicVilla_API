
using WebApi_test.Data;
using WebApi_test.Model;
using WebApi_test.Repository.IRepository;
namespace WebApi_test.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbcontext _dbcontext;
        public VillaNumberRepository(ApplicationDbcontext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;

        }
        public async Task<VillaNumber> UpdateAsync(VillaNumber entity)
        {
            entity.UpdateDate = DateTime.Now;
            _dbcontext.Update(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;

        }


    }
}
