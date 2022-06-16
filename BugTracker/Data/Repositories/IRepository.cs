using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.Data.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAllObjects();
        Task<T> GetObjectById(int id);
        Task<T> Create(T entity);
        Task<T> Update(T item);
        Task<T> Delete(int id);
    }
}