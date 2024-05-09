using System.Linq.Expressions;
using WebApi_test.Model;

namespace WebApi_test.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {

        Task<Villa> UpdateAsync(Villa entity);

    }
}
