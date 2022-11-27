using System.Linq.Expressions;
using VirtualDars.CachingDemo.Entities;

namespace VirtualDars.CachingDemo.Infra
{
    public interface IRepository<T> where T : IEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
    }
}
