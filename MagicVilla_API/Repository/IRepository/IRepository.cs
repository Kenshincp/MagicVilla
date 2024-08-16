using System.Linq.Expressions;

namespace MagicVilla_API.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entidad);
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> GetOne(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task Remove(T entidad);
        Task Save();

    }
}
