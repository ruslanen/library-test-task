using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library_test_task.Data
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(long id);

        IQueryable<T> GetAll();

        Task<long> SaveAsync(T value);

        Task DeleteAsync(long id);
    }
}